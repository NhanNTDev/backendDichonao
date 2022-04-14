using AutoMapper.QueryableExtensions;
using DiCho.Core.Custom;
using DiCho.DataService.Models;
using DiCho.DataService.ViewModels;
using Google.OrTools.ConstraintSolver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.Services
{
    public partial interface IVehicleRoutingService
    {
        Task CreateShipmentForRoutingProblem1(int warehouseId);
        Task<VehicleRoutingViewModel1> VehicleRouting1(int vehicleNumber);
        Task<CollectionOfFarmOrderModel> GetFarmsCollection(int warehouseId);
    }

    public partial class VehicleRoutingService : IVehicleRoutingService
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private readonly AutoMapper.IConfigurationProvider _mapper;
        private readonly IOrderService _orderService;
        private readonly IFarmOrderService _farmOrderService;
        private readonly IFarmService _farmService;
        private readonly IWareHouseService _wareHouseService;
        private readonly IShipmentService _shipmentService;
        private readonly IShipmentDestinationService _shipmentDestinationService;
        private readonly double binCapacity = 500;
        public VehicleRoutingService(IOrderService orderService, IShipmentDestinationService shipmentDestinationService, Microsoft.Extensions.Configuration.IConfiguration configuration,
            IFarmService farmService, IFarmOrderService farmOrderService, IWareHouseService wareHouseService, IShipmentService shipmentService, AutoMapper.IMapper mapper = null)
        {
            _mapper = mapper.ConfigurationProvider;
            _orderService = orderService;
            _configuration = configuration;
            _wareHouseService = wareHouseService;
            _shipmentService = shipmentService;
            _shipmentDestinationService = shipmentDestinationService;
            _farmOrderService = farmOrderService;
            _farmService = farmService;
        }

        public async Task CreateShipmentForRoutingProblem1(int warehouseId)
        {
            var orderGroups = await _orderService.GetOrderGroup(warehouseId);

            if (orderGroups.Count == 0)
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Không có đơn hàng nào cần được tạo chuyến hàng!");
            var weights = orderGroups.Sum(x => x.TotalWeight);
            var vehicleNumberEstimate = Math.Ceiling(weights / binCapacity);
            var warehouseFrom = _wareHouseService.Get(x => x.Id == warehouseId).FirstOrDefault();

            var addresses = new Dictionary<string, double>
            {
                { warehouseFrom.Address, 0 }
            };
            foreach (var orderGroup in orderGroups)
                addresses.Add(orderGroup.WarehouseAddress, orderGroup.TotalWeight);

            var routingFirst = await VehicleRouting(addresses, (int)vehicleNumberEstimate);
            routingFirst.RouteOfVihicles = routingFirst.RouteOfVihicles.Where(x => x.TotalWeightOfVihicle > 0).ToList();

            var warehouseDataRouting = new List<WarehouseDataRouting>();
            int vehicleNumberActual = 0;
            int vehicleNumber = (int)vehicleNumberEstimate;
            if (routingFirst.TotalWeight <= binCapacity)
            {
                var dataWeights = new List<double>();
                var dataOrderIds = new List<int>();
                var dataAddresses = orderGroups.Select(x => x.WarehouseAddress).ToList();
                foreach (var orderGroup in orderGroups)
                {
                    foreach (var orderGroupZone in orderGroup.OrderGroupZones)
                    {
                        dataWeights.AddRange(orderGroupZone.Orders.Select(x => x.Weight));
                        dataOrderIds.AddRange(orderGroupZone.Orders.Select(x => x.Id));
                    }
                }
                await _shipmentService.CreateShipment(dataWeights, dataOrderIds, dataAddresses, warehouseId);
            }
            else if (routingFirst.TotalWeight > binCapacity)
            {
                while (vehicleNumber > 1)
                {
                    foreach (var routeOfVihicle in routingFirst.RouteOfVihicles)
                    {
                        if (routeOfVihicle.TotalWeightOfVihicle <= binCapacity)
                        {
                            vehicleNumberActual += 1;
                            var addressesWarehouse = routeOfVihicle.Routes.Point.Keys.ToList();
                            addressesWarehouse.Remove(warehouseFrom.Address);
                            warehouseDataRouting.Add(new WarehouseDataRouting { WarehouseAddresses = addressesWarehouse });
                        }
                        else if (routeOfVihicle.TotalWeightOfVihicle > binCapacity && routeOfVihicle.Routes.Point.Count == 2)
                        {
                            var dataAddresses = routeOfVihicle.Routes.Point.Keys.ToList();
                            dataAddresses.Remove(warehouseFrom.Address);

                            var dataWeights = new List<double>();
                            var dataOrderIds = new List<int>();
                            foreach (var orderGroup in orderGroups)
                            {
                                foreach (var orderGroupZone in orderGroup.OrderGroupZones)
                                {
                                    if (dataAddresses.Any(x => x == orderGroup.WarehouseAddress))
                                    {
                                        dataWeights.AddRange(orderGroupZone.Orders.Select(x => x.Weight));
                                        dataOrderIds.AddRange(orderGroupZone.Orders.Select(x => x.Id));
                                    }
                                }
                            }
                            var binPackingNext = _orderService.BinPackingMip(dataWeights.ToArray(), dataOrderIds.ToArray());
                            vehicleNumberActual += (int)Math.Ceiling(binPackingNext.TotalBin);
                            warehouseDataRouting.Add(new WarehouseDataRouting { WarehouseAddresses = dataAddresses });
                        }
                        else if (routeOfVihicle.TotalWeightOfVihicle > binCapacity && routeOfVihicle.Routes.Point.Count > 2 && routingFirst.RouteOfVihicles.Where(x => x.Routes.Point.Count > 2).Count() == 1)
                        {
                            var vehicleNumberNext = vehicleNumber/*Math.Ceiling(routeOfVihicle.TotalWeightOfVihicle / binCapacity)*/;
                            var addressesNext = new Dictionary<string, double>();
                            foreach (var orderGroup in routeOfVihicle.Routes.Point)
                                addressesNext.Add(orderGroup.Key, orderGroup.Value);
                            routingFirst = await VehicleRouting(addressesNext, (int)vehicleNumberNext);
                            var number = routingFirst.RouteOfVihicles = routingFirst.RouteOfVihicles.Where(x => x.TotalWeightOfVihicle > 0).ToList();

                            vehicleNumber = number.Count;

                            if (vehicleNumber == 1)
                            {
                                if (routingFirst.TotalWeight > binCapacity)
                                {
                                    var endAddress = routingFirst.RouteOfVihicles.Select(x => x.Routes.Point);
                                    var dataEndAddress = new List<string>();
                                    foreach (var item in endAddress)
                                    {
                                        dataEndAddress.AddRange(item.Keys.ToList());
                                    }
                                    dataEndAddress.Remove(warehouseFrom.Address);
                                    warehouseDataRouting.Add(new WarehouseDataRouting { WarehouseAddresses = dataEndAddress });

                                    var dataWeights = new List<double>();
                                    var dataOrderIds = new List<int>();
                                    foreach (var orderGroup in orderGroups)
                                    {
                                        foreach (var orderGroupZone in orderGroup.OrderGroupZones)
                                        {
                                            if (dataEndAddress.Any(x => x == orderGroup.WarehouseAddress))
                                            {
                                                dataWeights.AddRange(orderGroupZone.Orders.Select(x => x.Weight));
                                                dataOrderIds.AddRange(orderGroupZone.Orders.Select(x => x.Id));
                                            }
                                        }
                                    }
                                    var binPackingNext = _orderService.BinPackingMip(dataWeights.ToArray(), dataOrderIds.ToArray());
                                    vehicleNumberActual += (int)Math.Ceiling(binPackingNext.TotalBin);
                                }
                                else if (routingFirst.TotalWeight < binCapacity)
                                {
                                    vehicleNumberActual += 1;
                                    var endAddress = routingFirst.RouteOfVihicles.Select(x => x.Routes.Point);
                                    var dataEndAddress = new List<string>();
                                    foreach (var item in endAddress)
                                    {
                                        dataEndAddress.AddRange(item.Keys.ToList());
                                    }
                                    dataEndAddress.Remove(warehouseFrom.Address);
                                    warehouseDataRouting.Add(new WarehouseDataRouting { WarehouseAddresses = dataEndAddress });
                                }
                            }
                        }
                        else if (routeOfVihicle.TotalWeightOfVihicle > binCapacity && routeOfVihicle.Routes.Point.Count > 2 && routingFirst.RouteOfVihicles.Where(x => x.Routes.Point.Count > 2).Count() > 1)
                        {
                            var vehicleNumberNext = vehicleNumber/*Math.Ceiling(routeOfVihicle.TotalWeightOfVihicle / binCapacity)*/;
                            var addressesNext = new Dictionary<string, double>();
                            foreach (var orderGroup in routeOfVihicle.Routes.Point)
                                addressesNext.Add(orderGroup.Key, orderGroup.Value);
                            routingFirst = await VehicleRouting(addressesNext, (int)vehicleNumberNext);
                            var number = routingFirst.RouteOfVihicles = routingFirst.RouteOfVihicles.Where(x => x.TotalWeightOfVihicle > 0).ToList();

                            vehicleNumber = number.Count /*(vehicleNumber - number.Count)*/;

                            if (vehicleNumber == 1)
                            {
                                if (routingFirst.TotalWeight > binCapacity)
                                {
                                    var endAddress = routingFirst.RouteOfVihicles.Select(x => x.Routes.Point);
                                    var dataEndAddress = new List<string>();
                                    foreach (var item in endAddress)
                                    {
                                        dataEndAddress.AddRange(item.Keys.ToList());
                                    }
                                    dataEndAddress.Remove(warehouseFrom.Address);
                                    warehouseDataRouting.Add(new WarehouseDataRouting { WarehouseAddresses = dataEndAddress });

                                    var dataWeights = new List<double>();
                                    var dataOrderIds = new List<int>();
                                    foreach (var orderGroup in orderGroups)
                                    {
                                        foreach (var orderGroupZone in orderGroup.OrderGroupZones)
                                        {
                                            if (dataEndAddress.Any(x => x == orderGroup.WarehouseAddress))
                                            {
                                                dataWeights.AddRange(orderGroupZone.Orders.Select(x => x.Weight));
                                                dataOrderIds.AddRange(orderGroupZone.Orders.Select(x => x.Id));
                                            }
                                        }
                                    }
                                    var binPackingNext = _orderService.BinPackingMip(dataWeights.ToArray(), dataOrderIds.ToArray());
                                    vehicleNumberActual += (int)Math.Ceiling(binPackingNext.TotalBin);
                                }
                                else if (routingFirst.TotalWeight < binCapacity)
                                {
                                    vehicleNumberActual += 1;
                                    var endAddress = routingFirst.RouteOfVihicles.Select(x => x.Routes.Point);
                                    var dataEndAddress = new List<string>();
                                    foreach (var item in endAddress)
                                    {
                                        dataEndAddress.AddRange(item.Keys.ToList());
                                    }
                                    dataEndAddress.Remove(warehouseFrom.Address);
                                    warehouseDataRouting.Add(new WarehouseDataRouting { WarehouseAddresses = dataEndAddress });
                                }
                            }
                        }
                    }
                }
            }

            if (vehicleNumberActual > vehicleNumberEstimate && vehicleNumberActual != 0)
            {

                var dataWeights = new List<double>();
                var dataOrderIds = new List<int>();
                foreach (var orderGroup in orderGroups)
                {
                    foreach (var orderGroupZone in orderGroup.OrderGroupZones)
                    {
                        dataWeights.AddRange(orderGroupZone.Orders.Select(x => x.Weight));
                        dataOrderIds.AddRange(orderGroupZone.Orders.Select(x => x.Id));
                    }
                }
                var binPacking = _orderService.BinPackingMip(dataWeights.ToArray(), dataOrderIds.ToArray());

                var date = DateTime.Now.ToString("ddMMyyyy");

                var lastShipment = _shipmentService.LastOrDefault();
                var codeShipment = 0;
                if (lastShipment == null)
                    codeShipment = Int32.Parse("1000");
                else
                    codeShipment = Int32.Parse(lastShipment.Code[13..]);
                foreach (var bin in binPacking.BinResult)
                {
                    var addressesWarehouse = new Dictionary<string, double>
                    {
                        { warehouseFrom.Address, 0 }
                    };
                    var orderIds = bin.BinDetail.Items;
                    foreach (var orderId in orderIds)
                    {
                        var add = orderGroups.Where(x => x.OrderGroupZones.Any(y => y.Orders.Any(z => z.Id == orderId))).FirstOrDefault().WarehouseAddress;
                        if (addressesWarehouse.Keys.ToList().All(x => x != add))
                            addressesWarehouse.Add(add, 0);
                    }
                    
                    var routing = await VehicleRouting(addressesWarehouse, 1);
                    foreach (var routeOfVihicle in routing.RouteOfVihicles)
                    {
                        var dataAddress = routeOfVihicle.Routes.Point.Keys.ToList();
                        dataAddress.Remove(warehouseFrom.Address);

                        codeShipment += 1;
                        var model = new ShipmentCreateModel
                        {
                            Code = "DCNS-" + date + codeShipment,
                            From = warehouseFrom.Address,
                            TotalWeight = bin.BinDetail.TotalWeight
                        };
                        var entity = _mapper.CreateMapper().Map<Shipment>(model);
                        await _shipmentService.CreateAsyn(entity);

                        foreach (var item in bin.BinDetail.Items)
                        {
                            var order = _orderService.Get(x => x.Id == item).FirstOrDefault();
                            order.ShipmentId = entity.Id;
                            await _orderService.UpdateAsyn(order);
                        }
                        foreach (var address in dataAddress)
                        {
                            var shipmentDestination = new ShipmentDestinationCreateModel { ShipmentId = entity.Id, Address = address };
                            var shipmentDestinationEntity = _mapper.CreateMapper().Map<ShipmentDestination>(shipmentDestination);
                            await _shipmentDestinationService.CreateAsyn(shipmentDestinationEntity);
                        }
                    }
                }
            }
            else if (vehicleNumberActual <= vehicleNumberEstimate && vehicleNumberActual != 0)
            {
                foreach (var warehouseAddress in warehouseDataRouting)
                {
                    var dataOrderGroups = orderGroups.Where(x => warehouseAddress.WarehouseAddresses.Any(y => y == x.WarehouseAddress));

                    var dataWeights = new List<double>();
                    var dataOrderIds = new List<int>();
                    var dataAddresses = warehouseAddress.WarehouseAddresses;
                    foreach (var dataOrderGroup in dataOrderGroups)
                    {
                        foreach (var orderGroupZone in dataOrderGroup.OrderGroupZones)
                        {
                            dataWeights.AddRange(orderGroupZone.Orders.Select(x => x.Weight));
                            dataOrderIds.AddRange(orderGroupZone.Orders.Select(x => x.Id));
                        }
                    }
                    await _shipmentService.CreateShipment(dataWeights, dataOrderIds, dataAddresses, warehouseId);
                }
            }
        }

        public async Task<CollectionOfFarmOrderModel> GetFarmsCollection(int warehouseId)
        {
            var farmGroups = await _orderService.GetGroupFarmOrderForDelivery(warehouseId);

            if (farmGroups.Count == 0){

            }
            var weights = farmGroups.Sum(x => x.TotalWeight);
            var vehicleNumberEstimate = Math.Ceiling(weights / binCapacity);
            var warehouseFrom = _wareHouseService.Get(x => x.Id == warehouseId).FirstOrDefault();

            var addresses = new Dictionary<string, double>
            {
                { warehouseFrom.Address, 0 }
            };
            foreach (var farmGroup in farmGroups)
                addresses.Add(farmGroup.FarmAddress, farmGroup.TotalWeight);

            var routingFirst = await VehicleRouting(addresses, (int)vehicleNumberEstimate);
            routingFirst.RouteOfVihicles = routingFirst.RouteOfVihicles.Where(x => x.TotalWeightOfVihicle > 0).ToList();

            
            var warehouseDataRouting = new List<WarehouseDataRouting>();
            //int vehicleNumberActual = 0;
            int vehicleNumber = (int)vehicleNumberEstimate;

            if (routingFirst.TotalWeight <= binCapacity)
            {
                var dataReturn = new CollectionOfFarmOrderModel();

                var farms = new List<FarmMapDataToViewGroupFarmOrder>();
                var dataFarmIds = farmGroups.Select(x => x.FarmId);
                var dataFarmOrdersIds = new List<int>();
                
                foreach (var farmGroup in farmGroups)
                    dataFarmOrdersIds.AddRange(farmGroup.FarmOrderGroups.Select(x => x.Id));

                //foreach (var farmOrderId in dataFarmOrdersIds)
                //{
                //    var entityFarmOrder = _farmOrderService.Get(x => x.Id == farmOrderId).FirstOrDefault();
                //    if (entityFarmOrder.CollectionCode == null)
                //    {
                //        var rnd = new Random();
                //        var collectionCode = "#" + DateTime.Now.ToString("ddMMyy") + "-" + rnd.Next(1000, 9999);
                //        entityFarmOrder.CollectionCode = collectionCode;
                //        await _farmOrderService.UpdateAsyn(entityFarmOrder);
                //    }
                //}

                foreach (var farmId in dataFarmIds)
                {
                    var farm = _farmService.Get(x => x.Id == farmId).ProjectTo<FarmMapDataToViewGroupFarmOrder>(_mapper).FirstOrDefault();
                    var farmOrder = new List<FarmOrderMapToGroupModel>();
                    foreach (var farmOrderId in dataFarmOrdersIds)
                    {
                        var dataFarmOrder = _farmOrderService.Get(x => x.Id == farmOrderId).ProjectTo<FarmOrderMapToGroupModel>(_mapper).FirstOrDefault();
                        dataReturn.CollectionCode = dataFarmOrder.CollectionCode;
                        if (dataFarmOrder.FarmId == farmId)
                            farmOrder.Add(dataFarmOrder);
                    }
                    farm.FarmOrders = farmOrder;
                    farms.Add(farm);
                }
                dataReturn.TotalWeight = weights;
                dataReturn.Farms = farms;
                return dataReturn;
            }
            return null;
            //else if (routingFirst.TotalWeight > binCapacity)
            //{
            //    while (vehicleNumber > 1)
            //    {
            //        foreach (var routeOfVihicle in routingFirst.RouteOfVihicles)
            //        {
            //            if (routeOfVihicle.TotalWeightOfVihicle <= binCapacity)
            //            {
            //                vehicleNumberActual += 1;
            //                var addressesWarehouse = routeOfVihicle.Routes.Point.Keys.ToList();
            //                addressesWarehouse.Remove(warehouseFrom.Address);
            //                warehouseDataRouting.Add(new WarehouseDataRouting { WarehouseAddresses = addressesWarehouse });
            //            }
            //            else if (routeOfVihicle.TotalWeightOfVihicle > binCapacity && routeOfVihicle.Routes.Point.Count == 2)
            //            {
            //                var dataAddresses = routeOfVihicle.Routes.Point.Keys.ToList();
            //                dataAddresses.Remove(warehouseFrom.Address);

            //                var dataWeights = new List<double>();
            //                var dataOrderIds = new List<int>();
            //                foreach (var orderGroup in orderGroups)
            //                {
            //                    foreach (var orderGroupZone in orderGroup.OrderGroupZones)
            //                    {
            //                        if (dataAddresses.Any(x => x == orderGroup.WarehouseAddress))
            //                        {
            //                            dataWeights.AddRange(orderGroupZone.Orders.Select(x => x.Weight));
            //                            dataOrderIds.AddRange(orderGroupZone.Orders.Select(x => x.Id));
            //                        }
            //                    }
            //                }
            //                var binPackingNext = _orderService.BinPackingMip(dataWeights.ToArray(), dataOrderIds.ToArray());
            //                vehicleNumberActual += (int)Math.Ceiling(binPackingNext.TotalBin);
            //                warehouseDataRouting.Add(new WarehouseDataRouting { WarehouseAddresses = dataAddresses });
            //            }
            //            else if (routeOfVihicle.TotalWeightOfVihicle > binCapacity && routeOfVihicle.Routes.Point.Count > 2 && routingFirst.RouteOfVihicles.Where(x => x.Routes.Point.Count > 2).Count() == 1)
            //            {
            //                var vehicleNumberNext = vehicleNumber/*Math.Ceiling(routeOfVihicle.TotalWeightOfVihicle / binCapacity)*/;
            //                var addressesNext = new Dictionary<string, double>();
            //                foreach (var orderGroup in routeOfVihicle.Routes.Point)
            //                    addressesNext.Add(orderGroup.Key, orderGroup.Value);
            //                routingFirst = await VehicleRouting(addressesNext, (int)vehicleNumberNext);
            //                var number = routingFirst.RouteOfVihicles = routingFirst.RouteOfVihicles.Where(x => x.TotalWeightOfVihicle > 0).ToList();

            //                vehicleNumber = number.Count;

            //                if (vehicleNumber == 1)
            //                {
            //                    if (routingFirst.TotalWeight > binCapacity)
            //                    {
            //                        var endAddress = routingFirst.RouteOfVihicles.Select(x => x.Routes.Point);
            //                        var dataEndAddress = new List<string>();
            //                        foreach (var item in endAddress)
            //                        {
            //                            dataEndAddress.AddRange(item.Keys.ToList());
            //                        }
            //                        dataEndAddress.Remove(warehouseFrom.Address);
            //                        warehouseDataRouting.Add(new WarehouseDataRouting { WarehouseAddresses = dataEndAddress });

            //                        var dataWeights = new List<double>();
            //                        var dataOrderIds = new List<int>();
            //                        foreach (var orderGroup in orderGroups)
            //                        {
            //                            foreach (var orderGroupZone in orderGroup.OrderGroupZones)
            //                            {
            //                                if (dataEndAddress.Any(x => x == orderGroup.WarehouseAddress))
            //                                {
            //                                    dataWeights.AddRange(orderGroupZone.Orders.Select(x => x.Weight));
            //                                    dataOrderIds.AddRange(orderGroupZone.Orders.Select(x => x.Id));
            //                                }
            //                            }
            //                        }
            //                        var binPackingNext = _orderService.BinPackingMip(dataWeights.ToArray(), dataOrderIds.ToArray());
            //                        vehicleNumberActual += (int)Math.Ceiling(binPackingNext.TotalBin);
            //                    }
            //                    else if (routingFirst.TotalWeight < binCapacity)
            //                    {
            //                        vehicleNumberActual += 1;
            //                        var endAddress = routingFirst.RouteOfVihicles.Select(x => x.Routes.Point);
            //                        var dataEndAddress = new List<string>();
            //                        foreach (var item in endAddress)
            //                        {
            //                            dataEndAddress.AddRange(item.Keys.ToList());
            //                        }
            //                        dataEndAddress.Remove(warehouseFrom.Address);
            //                        warehouseDataRouting.Add(new WarehouseDataRouting { WarehouseAddresses = dataEndAddress });
            //                    }
            //                }
            //            }
            //            else if (routeOfVihicle.TotalWeightOfVihicle > binCapacity && routeOfVihicle.Routes.Point.Count > 2 && routingFirst.RouteOfVihicles.Where(x => x.Routes.Point.Count > 2).Count() > 1)
            //            {
            //                var vehicleNumberNext = vehicleNumber/*Math.Ceiling(routeOfVihicle.TotalWeightOfVihicle / binCapacity)*/;
            //                var addressesNext = new Dictionary<string, double>();
            //                foreach (var orderGroup in routeOfVihicle.Routes.Point)
            //                    addressesNext.Add(orderGroup.Key, orderGroup.Value);
            //                routingFirst = await VehicleRouting(addressesNext, (int)vehicleNumberNext);
            //                var number = routingFirst.RouteOfVihicles = routingFirst.RouteOfVihicles.Where(x => x.TotalWeightOfVihicle > 0).ToList();

            //                vehicleNumber = number.Count /*(vehicleNumber - number.Count)*/;

            //                if (vehicleNumber == 1)
            //                {
            //                    if (routingFirst.TotalWeight > binCapacity)
            //                    {
            //                        var endAddress = routingFirst.RouteOfVihicles.Select(x => x.Routes.Point);
            //                        var dataEndAddress = new List<string>();
            //                        foreach (var item in endAddress)
            //                        {
            //                            dataEndAddress.AddRange(item.Keys.ToList());
            //                        }
            //                        dataEndAddress.Remove(warehouseFrom.Address);
            //                        warehouseDataRouting.Add(new WarehouseDataRouting { WarehouseAddresses = dataEndAddress });

            //                        var dataWeights = new List<double>();
            //                        var dataOrderIds = new List<int>();
            //                        foreach (var orderGroup in orderGroups)
            //                        {
            //                            foreach (var orderGroupZone in orderGroup.OrderGroupZones)
            //                            {
            //                                if (dataEndAddress.Any(x => x == orderGroup.WarehouseAddress))
            //                                {
            //                                    dataWeights.AddRange(orderGroupZone.Orders.Select(x => x.Weight));
            //                                    dataOrderIds.AddRange(orderGroupZone.Orders.Select(x => x.Id));
            //                                }
            //                            }
            //                        }
            //                        var binPackingNext = _orderService.BinPackingMip(dataWeights.ToArray(), dataOrderIds.ToArray());
            //                        vehicleNumberActual += (int)Math.Ceiling(binPackingNext.TotalBin);
            //                    }
            //                    else if (routingFirst.TotalWeight < binCapacity)
            //                    {
            //                        vehicleNumberActual += 1;
            //                        var endAddress = routingFirst.RouteOfVihicles.Select(x => x.Routes.Point);
            //                        var dataEndAddress = new List<string>();
            //                        foreach (var item in endAddress)
            //                        {
            //                            dataEndAddress.AddRange(item.Keys.ToList());
            //                        }
            //                        dataEndAddress.Remove(warehouseFrom.Address);
            //                        warehouseDataRouting.Add(new WarehouseDataRouting { WarehouseAddresses = dataEndAddress });
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            //if (vehicleNumberActual > vehicleNumberEstimate && vehicleNumberActual != 0)
            //{

            //    var dataWeights = new List<double>();
            //    var dataOrderIds = new List<int>();
            //    foreach (var orderGroup in orderGroups)
            //    {
            //        foreach (var orderGroupZone in orderGroup.OrderGroupZones)
            //        {
            //            dataWeights.AddRange(orderGroupZone.Orders.Select(x => x.Weight));
            //            dataOrderIds.AddRange(orderGroupZone.Orders.Select(x => x.Id));
            //        }
            //    }
            //    var binPacking = _orderService.BinPackingMip(dataWeights.ToArray(), dataOrderIds.ToArray());

            //    var date = DateTime.Now.ToString("ddMMyyyy");

            //    var lastShipment = _shipmentService.LastOrDefault();
            //    var codeShipment = 0;
            //    if (lastShipment == null)
            //        codeShipment = Int32.Parse("1000");
            //    else
            //        codeShipment = Int32.Parse(lastShipment.Code[13..]);
            //    foreach (var bin in binPacking.BinResult)
            //    {
            //        var addressesWarehouse = new Dictionary<string, double>
            //        {
            //            { warehouseFrom.Address, 0 }
            //        };
            //        var orderIds = bin.BinDetail.Items;
            //        foreach (var orderId in orderIds)
            //        {
            //            var add = orderGroups.Where(x => x.OrderGroupZones.Any(y => y.Orders.Any(z => z.Id == orderId))).FirstOrDefault().WarehouseAddress;
            //            if (addressesWarehouse.Keys.ToList().All(x => x != add))
            //                addressesWarehouse.Add(add, 0);
            //        }

            //        var routing = await VehicleRouting(addressesWarehouse, 1);
            //        foreach (var routeOfVihicle in routing.RouteOfVihicles)
            //        {
            //            var dataAddress = routeOfVihicle.Routes.Point.Keys.ToList();
            //            dataAddress.Remove(warehouseFrom.Address);

            //            codeShipment += 1;
            //            var model = new ShipmentCreateModel
            //            {
            //                Code = "DCNS-" + date + codeShipment,
            //                From = warehouseFrom.Address,
            //                TotalWeight = bin.BinDetail.TotalWeight
            //            };
            //            var entity = _mapper.CreateMapper().Map<Shipment>(model);
            //            await _shipmentService.CreateAsyn(entity);

            //            foreach (var item in bin.BinDetail.Items)
            //            {
            //                var order = _orderService.Get(x => x.Id == item).FirstOrDefault();
            //                order.ShipmentId = entity.Id;
            //                await _orderService.UpdateAsyn(order);
            //            }
            //            foreach (var address in dataAddress)
            //            {
            //                var shipmentDestination = new ShipmentDestinationCreateModel { ShipmentId = entity.Id, Address = address };
            //                var shipmentDestinationEntity = _mapper.CreateMapper().Map<ShipmentDestination>(shipmentDestination);
            //                await _shipmentDestinationService.CreateAsyn(shipmentDestinationEntity);
            //            }
            //        }
            //    }
            //}
            //else if (vehicleNumberActual <= vehicleNumberEstimate && vehicleNumberActual != 0)
            //{
            //    foreach (var warehouseAddress in warehouseDataRouting)
            //    {
            //        var dataOrderGroups = orderGroups.Where(x => warehouseAddress.WarehouseAddresses.Any(y => y == x.WarehouseAddress));

            //        var dataWeights = new List<double>();
            //        var dataOrderIds = new List<int>();
            //        var dataAddresses = warehouseAddress.WarehouseAddresses;
            //        foreach (var dataOrderGroup in dataOrderGroups)
            //        {
            //            foreach (var orderGroupZone in dataOrderGroup.OrderGroupZones)
            //            {
            //                dataWeights.AddRange(orderGroupZone.Orders.Select(x => x.Weight));
            //                dataOrderIds.AddRange(orderGroupZone.Orders.Select(x => x.Id));
            //            }
            //        }
            //        await _shipmentService.CreateShipment(dataWeights, dataOrderIds, dataAddresses, warehouseId);
            //    }
            //}
        }

        public async Task CreateShipmentForRoutingProblem(int warehouseId)
        {
            var orderGroups = await _orderService.GetOrderGroup(warehouseId);
            var weights = orderGroups.Sum(x => x.TotalWeight);
            var vehicleNumberEstimate = Math.Ceiling(weights / binCapacity);
            var warehouseFrom = _wareHouseService.Get(x => x.Id == warehouseId).FirstOrDefault();

            var addresses = new Dictionary<string, double>
            {
                { warehouseFrom.Address, 0 }
            };
            foreach (var orderGroup in orderGroups)
                addresses.Add(orderGroup.WarehouseAddress, orderGroup.TotalWeight);

            var routingFirst = await VehicleRouting(addresses, (int)vehicleNumberEstimate);
            routingFirst.RouteOfVihicles = routingFirst.RouteOfVihicles.Where(x => x.TotalWeightOfVihicle > 0).ToList();

            var warehouseDataRouting = new List<WarehouseDataRouting>();
            int vehicleNumberActual = 0;
            if (routingFirst.TotalWeight < binCapacity)
            {
                var dataWeights = new List<double>();
                var dataOrderIds = new List<int>();
                var dataAddresses = orderGroups.Select(x => x.WarehouseAddress).ToList();
                foreach (var orderGroup in orderGroups)
                {
                    foreach (var orderGroupZone in orderGroup.OrderGroupZones)
                    {
                        dataWeights.AddRange(orderGroupZone.Orders.Select(x => x.Weight));
                        dataOrderIds.AddRange(orderGroupZone.Orders.Select(x => x.Id));
                    }
                }
                await _shipmentService.CreateShipment(dataWeights, dataOrderIds, dataAddresses, warehouseId);
            }
            else if (routingFirst.TotalWeight > binCapacity)
            {
                foreach (var routeOfVihicle in routingFirst.RouteOfVihicles)
                {
                    if (routeOfVihicle.TotalWeightOfVihicle < binCapacity)
                    {
                        vehicleNumberActual += 1;
                        var addressesWarehouse = routeOfVihicle.Routes.Point.Keys.ToList();
                        addressesWarehouse.Remove(warehouseFrom.Address);
                        warehouseDataRouting.Add(new WarehouseDataRouting { WarehouseAddresses = addressesWarehouse });
                    }
                    else if (routeOfVihicle.TotalWeightOfVihicle > binCapacity && routeOfVihicle.Routes.Point.Count == 2)
                    {
                        var dataAddresses = routeOfVihicle.Routes.Point.Keys.ToList();
                        dataAddresses.Remove(warehouseFrom.Address);

                        var dataWeights = new List<double>();
                        var dataOrderIds = new List<int>();
                        foreach (var orderGroup in orderGroups)
                        {
                            foreach (var orderGroupZone in orderGroup.OrderGroupZones)
                            {
                                if (dataAddresses.Any(x => x == orderGroup.WarehouseAddress))
                                {
                                    dataWeights.AddRange(orderGroupZone.Orders.Select(x => x.Weight));
                                    dataOrderIds.AddRange(orderGroupZone.Orders.Select(x => x.Id));
                                }
                            }
                        }
                        var binPackingNext = _orderService.BinPackingMip(dataWeights.ToArray(), dataOrderIds.ToArray());
                        vehicleNumberActual += (int)Math.Ceiling(binPackingNext.TotalBin);
                        warehouseDataRouting.Add(new WarehouseDataRouting { WarehouseAddresses = dataAddresses });
                    }
                    else if (routeOfVihicle.TotalWeightOfVihicle > binCapacity && routeOfVihicle.Routes.Point.Count > 2)
                    {
                        var vehicleNumberNext = Math.Ceiling(routeOfVihicle.TotalWeightOfVihicle / binCapacity);
                        var addressesNext = new Dictionary<string, double>();
                        foreach (var orderGroup in routeOfVihicle.Routes.Point)
                            addressesNext.Add(orderGroup.Key, orderGroup.Value);
                        var routingNext = await VehicleRouting(addressesNext, (int)vehicleNumberNext);
                        routingNext.RouteOfVihicles = routingNext.RouteOfVihicles.Where(x => x.TotalWeightOfVihicle > 0).ToList();

                        if (routingNext.TotalWeight < binCapacity)
                        {

                        }
                        else if (routingNext.TotalWeight > binCapacity)
                        {
                            foreach (var routeOfVihiclesNext in routingNext.RouteOfVihicles)
                            {
                                if (routeOfVihiclesNext.TotalWeightOfVihicle < binCapacity)
                                {
                                    vehicleNumberActual += 1;
                                    var addressesWarehouse = routeOfVihiclesNext.Routes.Point.Keys.ToList();
                                    addressesWarehouse.Remove(warehouseFrom.Address);
                                    warehouseDataRouting.Add(new WarehouseDataRouting { WarehouseAddresses = addressesWarehouse });
                                }
                                else if (routeOfVihiclesNext.TotalWeightOfVihicle > binCapacity && routeOfVihiclesNext.Routes.Point.Count == 2)
                                {
                                    var dataAddresses = routeOfVihiclesNext.Routes.Point.Keys.ToList();
                                    dataAddresses.Remove(warehouseFrom.Address);

                                    var dataWeights = new List<double>();
                                    var dataOrderIds = new List<int>();
                                    foreach (var orderGroup in orderGroups)
                                    {
                                        foreach (var orderGroupZone in orderGroup.OrderGroupZones)
                                        {
                                            if (dataAddresses.Any(x => x == orderGroup.WarehouseAddress))
                                            {
                                                dataWeights.AddRange(orderGroupZone.Orders.Select(x => x.Weight));
                                                dataOrderIds.AddRange(orderGroupZone.Orders.Select(x => x.Id));
                                            }
                                        }
                                    }
                                    var binPackingNext = _orderService.BinPackingMip(dataWeights.ToArray(), dataOrderIds.ToArray());
                                    vehicleNumberActual += (int)Math.Ceiling(binPackingNext.TotalBin);
                                    warehouseDataRouting.Add(new WarehouseDataRouting { WarehouseAddresses = dataAddresses });
                                }
                                //else if (routeOfVihiclesNext.TotalWeightOfVihicle > binCapacity && routeOfVihiclesNext.Routes.Point.Count > 2)
                                //{

                                //}
                            }
                        }
                    }
                }
            }

            if (vehicleNumberActual > vehicleNumberEstimate && vehicleNumberActual != 0)
            {
                var dataWeights = new List<double>();
                var dataOrderIds = new List<int>();
                var dataAddresses = orderGroups.Select(x => x.WarehouseAddress).ToList();
                foreach (var orderGroup in orderGroups)
                {
                    foreach (var orderGroupZone in orderGroup.OrderGroupZones)
                    {
                        dataWeights.AddRange(orderGroupZone.Orders.Select(x => x.Weight));
                        dataOrderIds.AddRange(orderGroupZone.Orders.Select(x => x.Id));
                    }
                }
                var binPacking = _orderService.BinPackingMip(dataWeights.ToArray(), dataOrderIds.ToArray());

                var date = DateTime.Now.ToString("ddMMyyyy");

                var lastShipment = _shipmentService.LastOrDefault();
                var codeShipment = 0;
                if (lastShipment == null)
                    codeShipment = Int32.Parse("1000");
                else
                    codeShipment = Int32.Parse(lastShipment.Code[13..]);
                foreach (var bin in binPacking.BinResult)
                {
                    var addressesWarehouse = new Dictionary<string, double>
                    {
                        { warehouseFrom.Address, 0 }
                    };
                    var orderIds = bin.BinDetail.Items;
                    foreach (var orderGroup in orderGroups)
                    {
                        foreach (var orderGroupZone in orderGroup.OrderGroupZones)
                        {
                            double sum = 0;
                            foreach (var orderId in orderIds)
                            {
                                if (orderGroupZone.Orders.Any(x => x.Id == orderId))
                                {
                                    sum += orderGroupZone.Orders.Where(x => x.Id == orderId).FirstOrDefault().Weight;
                                }
                            }
                            if (addressesWarehouse.Keys.Any(x => x != orderGroup.WarehouseAddress))
                                addressesWarehouse.Add(orderGroup.WarehouseAddress, sum);
                        }
                    }
                    var routing = await VehicleRouting(addressesWarehouse, 1);
                    foreach (var routeOfVihicle in routing.RouteOfVihicles)
                    {
                        var dataAddress = routeOfVihicle.Routes.Point.Keys.ToList();
                        dataAddress.Remove(warehouseFrom.Address);

                        codeShipment += 1;
                        var model = new ShipmentCreateModel
                        {
                            Code = "DCNS-" + date + codeShipment,
                            From = warehouseFrom.Address,
                            TotalWeight = bin.BinDetail.TotalWeight
                        };
                        var entity = _mapper.CreateMapper().Map<Shipment>(model);
                        await _shipmentService.CreateAsyn(entity);

                        foreach (var item in bin.BinDetail.Items)
                        {
                            var order = _orderService.Get(x => x.Id == item).FirstOrDefault();
                            order.ShipmentId = entity.Id;
                            await _orderService.UpdateAsyn(order);
                        }
                        foreach (var address in dataAddress)
                        {
                            var shipmentDestination = new ShipmentDestinationCreateModel { ShipmentId = entity.Id, Address = address };
                            var shipmentDestinationEntity = _mapper.CreateMapper().Map<ShipmentDestination>(shipmentDestination);
                            await _shipmentDestinationService.CreateAsyn(shipmentDestinationEntity);
                        }
                    }
                }
            }
            else if (vehicleNumberActual <= vehicleNumberEstimate && vehicleNumberActual != 0)
            {
                foreach (var warehouseAddress in warehouseDataRouting)
                {
                    var dataOrderGroups = orderGroups.Where(x => warehouseAddress.WarehouseAddresses.Any(y => y == x.WarehouseAddress));

                    var dataWeights = new List<double>();
                    var dataOrderIds = new List<int>();
                    var dataAddresses = warehouseAddress.WarehouseAddresses;
                    foreach (var dataOrderGroup in dataOrderGroups)
                    {
                        foreach (var orderGroupZone in dataOrderGroup.OrderGroupZones)
                        {
                            dataWeights.AddRange(orderGroupZone.Orders.Select(x => x.Weight));
                            dataOrderIds.AddRange(orderGroupZone.Orders.Select(x => x.Id));
                        }
                    }
                    await _shipmentService.CreateShipment(dataWeights, dataOrderIds, dataAddresses, warehouseId);
                }
            }
        }

        private static VehicleRoutingViewModel PrintSolution(in VehicleRoutingModel data, in RoutingModel routing, in RoutingIndexManager manager,
                              in Assignment solution, Dictionary<string, double> addresses)
        {
            var vehicleRouting = new VehicleRoutingViewModel();
            var routeOfVihicles = new List<RouteOfVihicle>();
            // Inspect solution.
            long maxRouteDistance = 0;
            int totalDistance = 0;
            var dataAddress = addresses.Keys.ToArray();
            var dataWeight = addresses.Values.ToArray();
            for (int i = 0; i < data.VehicleNumber; ++i)
            {
                var routes = new Routes();
                long routeDistance = 0;
                var index = routing.Start(i);
                var points = new Dictionary<string, double>();
                while (routing.IsEnd(index) == false)
                {
                    if (points.Count == 0)
                        points.Add(dataAddress[0], 0);
                    else
                        points.Add(dataAddress[index], dataWeight[index]);
                    var previousIndex = index;
                    index = solution.Value(routing.NextVar(index));
                    routeDistance += routing.GetArcCostForVehicle(previousIndex, index, 0);
                }
                maxRouteDistance = Math.Max(routeDistance, maxRouteDistance);

                totalDistance += (int)routeDistance;
                routes.Point = points;
                routes.Distance = routeDistance.ToString() + "m";
                routeOfVihicles.Add(new RouteOfVihicle { Vihicle = i, Routes = routes, TotalWeightOfVihicle = points.Sum(x => x.Value) });
            }
            vehicleRouting.TotalDistance = totalDistance + "m";
            vehicleRouting.RouteOfVihicles = routeOfVihicles;
            vehicleRouting.TotalWeight = routeOfVihicles.Sum(x => x.TotalWeightOfVihicle);

            return vehicleRouting;
        }

        public async Task<VehicleRoutingViewModel> VehicleRouting(Dictionary<string, double> addresses, int vehicleNumber)
        {
            var distanceMatrix = await Create_distance_matrix(addresses.Keys.ToArray());
            VehicleRoutingModel data = new() { VehicleNumber = vehicleNumber, Depot = 0, DistanceMatrix = distanceMatrix };

            RoutingIndexManager manager =
                new(data.DistanceMatrix.GetLength(0), data.VehicleNumber, data.Depot);

            RoutingModel routing = new(manager);

            int transitCallbackIndex = routing.RegisterTransitCallback((long fromIndex, long toIndex) =>
            {
                var fromNode = manager.IndexToNode(fromIndex);
                var toNode = manager.IndexToNode(toIndex);
                return data.DistanceMatrix[fromNode, toNode];
            });

            routing.SetArcCostEvaluatorOfAllVehicles(transitCallbackIndex);

            routing.AddDimension(transitCallbackIndex, 0, 300000,
                                 true, // start cumul to zero
                                 "Distance");
            RoutingDimension distanceDimension = routing.GetMutableDimension("Distance");
            distanceDimension.SetGlobalSpanCostCoefficient(100);

            RoutingSearchParameters searchParameters =
                operations_research_constraint_solver.DefaultRoutingSearchParameters();
            searchParameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;

            Assignment solution = routing.SolveWithParameters(searchParameters);

            var printSolution = PrintSolution(data, routing, manager, solution, addresses);
            return printSolution;
        }

        static long[,] CreateRectangularArray(List<long[]> arrays)
        {
            int minorLength = arrays[0].Length;
            long[,] ret = new long[arrays.Count, minorLength];
            for (int i = 0; i < arrays.Count; i++)
            {
                var array = arrays[i];
                for (int j = 0; j < minorLength; j++)
                {
                    ret[i, j] = array[j];
                }
            }
            return ret;
        }

        public async Task<long[,]> Create_distance_matrix(string[] addresses)
        {
            DistanceMatrixModel response;
            string origin_addresses;
            var API_key = _configuration["Geocoding:ApiKey"];

            var num_addresses = addresses.Length;

            var dest_addresses = addresses;
            var distance_matrix = new List<long[]>();
            foreach (var i in Enumerable.Range(0, num_addresses))
            {
                origin_addresses = addresses[i];
                response = await Send_request(origin_addresses, dest_addresses, API_key);
                distance_matrix.Add(build_distance_matrix(response));
            }

            long[,] array = CreateRectangularArray(distance_matrix);

            return array;
        }

        public string build_address_str(string[] addresses)
        {
            var address_str = "";
            foreach (var i in Enumerable.Range(0, addresses.Length - 1))
            {
                address_str += addresses[i] + "|";
            }
            address_str += addresses[^1];
            return address_str;
        }

        public async Task<DistanceMatrixModel> Send_request(string origin_addresses, string[] dest_addresses, string API_key)
        {
            var origin_address_str = /*build_address_str*/(origin_addresses);
            var dest_address_str = build_address_str(dest_addresses);
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://maps.googleapis.com/");
            using HttpResponseMessage response = await client.GetAsync($"maps/api/distancematrix/json?units=imperial" + "&origins=" + origin_address_str + "&destinations=" + dest_address_str + "&key=" + API_key);
            var responseContent = response.Content.ReadAsStringAsync().Result;
            response.EnsureSuccessStatusCode();
            var distanceMatrix = JsonConvert.DeserializeObject<DistanceMatrixModel>(responseContent);
            return distanceMatrix;
        }

        public static long[] build_distance_matrix(DistanceMatrixModel distanceMatrix)
        {
            var distance_matrix = new List<long>();
            foreach (var row in distanceMatrix.Rows)
            {
                var row_list = row.Elements.Select(x => Convert.ToInt64(x.Distance.Value)).ToArray();
                distance_matrix.AddRange(row_list);
            }
            return distance_matrix.ToArray();
        }





        //
        private static VehicleRoutingViewModel1 PrintSolution1(in VehicleRoutingModel data, in RoutingModel routing, in RoutingIndexManager manager,
                              in Assignment solution)
        {
            var vehicleRouting = new VehicleRoutingViewModel1();
            var routeOfVihicles = new List<RouteOfVihicle1>();
            var address = new VehicleRoutingAddressModel();
            // Inspect solution.
            long maxRouteDistance = 0;
            int totalDistance = 0;
            var dataAddress = address.Address;
            for (int i = 0; i < data.VehicleNumber; ++i)
            {
                var routes = new Routes1();
                long routeDistance = 0;
                var index = routing.Start(i);
                var points = new List<string>();
                while (routing.IsEnd(index) == false)
                {
                    if (points.Count == 0)
                        points.Add(dataAddress[0]);
                    else
                        points.Add(dataAddress[index]);
                    var previousIndex = index;
                    index = solution.Value(routing.NextVar(index));
                    routeDistance += routing.GetArcCostForVehicle(previousIndex, index, 0);
                }
                maxRouteDistance = Math.Max(routeDistance, maxRouteDistance);

                totalDistance += (int)routeDistance;
                routes.Point = points;
                routes.Distance = routeDistance.ToString() + "m";
                routeOfVihicles.Add(new RouteOfVihicle1 { Vihicle = i, Routes = routes });
            }
            vehicleRouting.TotalDistance = totalDistance + "m";
            vehicleRouting.RouteOfVihicles = routeOfVihicles;

            return vehicleRouting;
        }

        public async Task<VehicleRoutingViewModel1> VehicleRouting1(int vehicleNumber)
        {
            var address = new VehicleRoutingAddressModel();

            var distanceMatrix = await Create_distance_matrix(address.Address);
            VehicleRoutingModel data = new() { VehicleNumber = vehicleNumber, Depot = 0, DistanceMatrix = distanceMatrix };

            RoutingIndexManager manager =
                new(data.DistanceMatrix.GetLength(0), data.VehicleNumber, data.Depot);

            RoutingModel routing = new(manager);

            int transitCallbackIndex = routing.RegisterTransitCallback((long fromIndex, long toIndex) =>
            {
                var fromNode = manager.IndexToNode(fromIndex);
                var toNode = manager.IndexToNode(toIndex);
                return data.DistanceMatrix[fromNode, toNode];
            });

            routing.SetArcCostEvaluatorOfAllVehicles(transitCallbackIndex);

            routing.AddDimension(transitCallbackIndex, 0, 300000,
                                 true, // start cumul to zero
                                 "Distance");
            RoutingDimension distanceDimension = routing.GetMutableDimension("Distance");
            distanceDimension.SetGlobalSpanCostCoefficient(100);

            RoutingSearchParameters searchParameters =
                operations_research_constraint_solver.DefaultRoutingSearchParameters();
            searchParameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;

            Assignment solution = routing.SolveWithParameters(searchParameters);

            var printSolution = PrintSolution1(data, routing, manager, solution);
            return printSolution;
        }
    }
}
