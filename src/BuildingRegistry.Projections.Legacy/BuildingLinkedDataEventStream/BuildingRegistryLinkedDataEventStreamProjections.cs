namespace BuildingRegistry.Projections.Legacy.BuildingLinkedDataEventStream
{
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Connector;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.SqlStreamStore;
    using Be.Vlaanderen.Basisregisters.Utilities.HexByteConvertor;
    using BuildingRegistry.Building.Events;
    using BuildingRegistry.Projections.Legacy.BuildingSyndication;
    using BuildingRegistry.ValueObjects;
    using Microsoft.EntityFrameworkCore;
    using NodaTime;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    [ConnectedProjectionName("Legacy - Building Linked Data Event Stream")]
    [ConnectedProjectionDescription("Gebouw en gebouweenheid data voor de Linked Data Event Stream.")]
    public class BuildingRegistryLinkedDataEventStreamProjections : ConnectedProjection<LegacyContext>
    {
        public BuildingRegistryLinkedDataEventStreamProjections()
        {
            #region Building Events

            When<Envelope<BuildingWasRegistered>>(async (context, message, ct) =>
            {
                var newBuildingLinkedDataEvenStreamItem = new BuildingLinkedDataEventStreamItem
                {
                    Position = message.Position,
                    BuildingId = message.Message.BuildingId,
                    EventGeneratedAtTime = message.Message.Provenance.Timestamp,
                    ChangeType = message.EventName
                };

                newBuildingLinkedDataEvenStreamItem.SetObjectHash();
                newBuildingLinkedDataEvenStreamItem.CheckIfRecordCanBePublished();

                await context
                    .BuildingLinkedDataEventStream
                    .AddAsync(newBuildingLinkedDataEvenStreamItem, ct);
            });

            When<Envelope<BuildingBecameComplete>>(async (context, message, ct) =>
            {
                // IGNORE
            });

            When<Envelope<BuildingBecameIncomplete>>(async (context, message, ct) =>
            {
                // IGNORE
            });

            When<Envelope<BuildingBecameUnderConstruction>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x => x.Status = BuildingStatus.UnderConstruction,
                    true,
                    ct);
            });

            When<Envelope<BuildingGeometryWasRemoved>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        x.GeometryMethod = null;
                        x.Geometry = null;
                    },
                    true,
                    ct);
            });

            When<Envelope<BuildingMeasurementByGrbWasCorrected>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        x.GeometryMethod = BuildingGeometryMethod.MeasuredByGrb;
                        x.Geometry = message.Message.ExtendedWkbGeometry.ToByteArray();
                    },
                    true,
                    ct);
            });

            When<Envelope<BuildingPersistentLocalIdWasAssigned>>(async (context, message, ct) =>
            {
                await context.UpdateBuildingPersistentLocalIdentifier(
                    message.Message.BuildingId,
                    message.Message.PersistentLocalId,
                    ct);
            });

            When<Envelope<BuildingOutlineWasCorrected>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        x.GeometryMethod = BuildingGeometryMethod.Outlined;
                        x.Geometry = message.Message.ExtendedWkbGeometry.ToByteArray();
                    },
                    true,
                    ct);
            });

            When<Envelope<BuildingStatusWasCorrectedToRemoved>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x => x.Status = null,
                    true,
                    ct);
            });

            When<Envelope<BuildingStatusWasRemoved>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x => x.Status = null,
                    true,
                    ct);
            });

            When<Envelope<BuildingWasCorrectedToNotRealized>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        x.Status = BuildingStatus.NotRealized;

                        RetireUnitsByBuilding(
                            x.BuildingUnits,
                            message.Message.BuildingUnitIdsToNotRealize,
                            message.Message.BuildingUnitIdsToRetire,
                            message.Message.Provenance.Timestamp,
                            message.EventName);
                    },
                    true,
                    ct);
            });

            When<Envelope<BuildingWasCorrectedToPlanned>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x => x.Status = BuildingStatus.Planned,
                    true,
                    ct);
            });

            When<Envelope<BuildingWasCorrectedToRealized>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x => x.Status = BuildingStatus.Realized,
                    true,
                    ct);
            });

            When<Envelope<BuildingWasCorrectedToRetired>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        x.Status = BuildingStatus.Retired;

                        RetireUnitsByBuilding(
                            x.BuildingUnits,
                            message.Message.BuildingUnitIdsToNotRealize,
                            message.Message.BuildingUnitIdsToRetire,
                            message.Message.Provenance.Timestamp,
                            message.EventName);
                    },
                    true,
                    ct);
            });

            When<Envelope<BuildingWasCorrectedToUnderConstruction>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x => x.Status = BuildingStatus.UnderConstruction,
                    true,
                    ct);
            });

            When<Envelope<BuildingWasMeasuredByGrb>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        x.GeometryMethod = BuildingGeometryMethod.MeasuredByGrb;
                        x.Geometry = message.Message.ExtendedWkbGeometry.ToByteArray();
                    },
                    true,
                    ct);
            });

            When<Envelope<BuildingWasNotRealized>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        x.Status = BuildingStatus.NotRealized;

                        RetireUnitsByBuilding(
                            x.BuildingUnits,
                            message.Message.BuildingUnitIdsToNotRealize,
                            message.Message.BuildingUnitIdsToRetire,
                            message.Message.Provenance.Timestamp,
                            message.EventName);
                    },
                    true,
                    ct);
            });

            When<Envelope<BuildingWasOutlined>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        x.GeometryMethod = BuildingGeometryMethod.Outlined;
                        x.Geometry = message.Message.ExtendedWkbGeometry.ToByteArray();
                    },
                    true,
                    ct);
            });

            When<Envelope<BuildingWasPlanned>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x => x.Status = BuildingStatus.Planned,
                    true,
                    ct);
            });

            When<Envelope<BuildingWasRealized>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x => x.Status = BuildingStatus.Realized,
                    true,
                    ct);
            });

            When<Envelope<BuildingWasRetired>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        x.Status = BuildingStatus.Retired;

                        RetireUnitsByBuilding(
                            x.BuildingUnits,
                            message.Message.BuildingUnitIdsToNotRealize,
                            message.Message.BuildingUnitIdsToRetire,
                            message.Message.Provenance.Timestamp,
                            message.EventName);
                    },
                    true,
                    ct);
            });

            When<Envelope<BuildingWasRemoved>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x => { },
                    true,
                    ct);

                await context.BuildingWasRemoved(
                    message.Message.BuildingId,
                    ct);
            });

            #endregion Building Events

            #region Building Unit Events

            When<Envelope<CommonBuildingUnitWasAdded>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        var buildingUnitLinkedDataEventStreamItem = new BuildingUnitLinkedDataEventStreamItem
                        {
                            Position = message.Position,
                            BuildingId = message.Message.BuildingId,
                            BuildingUnitId = message.Message.BuildingUnitId,
                            Function = BuildingUnitFunction.Common,
                            EventGeneratedAtTime = message.Message.Provenance.Timestamp,
                            ChangeType = message.EventName
                        };

                        buildingUnitLinkedDataEventStreamItem.SetObjectHash();
                        buildingUnitLinkedDataEventStreamItem.CheckIfRecordCanBePublished();

                        x.BuildingUnits.Add(buildingUnitLinkedDataEventStreamItem);
                    },
                    true,
                    ct);
            });

            When<Envelope<BuildingUnitWasAdded>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        var buildingUnitLinkedDataEventStreamItem = new BuildingUnitLinkedDataEventStreamItem
                        {
                            Position = message.Position,
                            BuildingId = message.Message.BuildingId,
                            BuildingUnitId = message.Message.BuildingUnitId,
                            Function = BuildingUnitFunction.Unknown,
                            EventGeneratedAtTime = message.Message.Provenance.Timestamp,
                            ChangeType = message.EventName,
                            Addresses =
                            {
                                new BuildingUnitAddressLinkedDataEventStreamItem
                                {
                                    BuildingUnitId = message.Message.BuildingUnitId,
                                    Position = message.Position,
                                    AddressId = message.Message.AddressId,
                                    Count = 1
                              }
                            }
                        };

                        buildingUnitLinkedDataEventStreamItem.SetObjectHash();
                        buildingUnitLinkedDataEventStreamItem.CheckIfRecordCanBePublished();

                        x.BuildingUnits.Add(buildingUnitLinkedDataEventStreamItem);
                    },
                    true,
                    ct);
            });

            When<Envelope<BuildingUnitWasAddedToRetiredBuilding>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    (oldLinkedDataEventStreamItem, newLinkedDataEventStreamItem) =>
                    {
                        var buildingUnitLinkedDataEventStreamItem = new BuildingUnitLinkedDataEventStreamItem
                        {
                            Position = message.Position,
                            BuildingId = message.Message.BuildingId,
                            BuildingUnitId = message.Message.BuildingUnitId,
                            Function = BuildingUnitFunction.Unknown,
                            EventGeneratedAtTime = message.Message.Provenance.Timestamp,
                            ChangeType = message.EventName,
                            Status = oldLinkedDataEventStreamItem.Status == BuildingStatus.NotRealized
                                ? BuildingUnitStatus.NotRealized
                                : BuildingUnitStatus.Retired
                        };

                        buildingUnitLinkedDataEventStreamItem.SetObjectHash();
                        buildingUnitLinkedDataEventStreamItem.CheckIfRecordCanBePublished();

                        newLinkedDataEventStreamItem.BuildingUnits.Add(buildingUnitLinkedDataEventStreamItem);
                    },
                    true,
                    ct);
            });

            When<Envelope<BuildingUnitWasReaddedByOtherUnitRemoval>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        var buildingUnitLinkedDataEventStreamItem = new BuildingUnitLinkedDataEventStreamItem
                        {
                            Position = message.Position,
                            BuildingId = message.Message.BuildingId,
                            BuildingUnitId = message.Message.BuildingUnitId,
                            Function = BuildingUnitFunction.Unknown,
                            EventGeneratedAtTime = message.Message.Provenance.Timestamp,
                            ChangeType = message.EventName,
                            Addresses =
                            {
                                new BuildingUnitAddressLinkedDataEventStreamItem
                                {
                                    BuildingUnitId = message.Message.BuildingUnitId,
                                    Position = message.Position,
                                    AddressId = message.Message.AddressId,
                                    Count = 1
                                }
                            }
                        };

                        buildingUnitLinkedDataEventStreamItem.SetObjectHash();
                        buildingUnitLinkedDataEventStreamItem.CheckIfRecordCanBePublished();

                        x.BuildingUnits.Add(buildingUnitLinkedDataEventStreamItem);
                    },
                    true,
                    ct);
            });

            When<Envelope<BuildingUnitWasRemoved>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x => x.BuildingUnits.Remove(x.BuildingUnits.FirstOrDefault(y => y.BuildingUnitId == message.Message.BuildingUnitId)),
                    true,
                    ct);
            });

            When<Envelope<BuildingUnitAddressWasAttached>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        var unit = x.BuildingUnits.Single(u => u.BuildingUnitId == message.Message.To);

                        if (unit.Addresses.Any(u => u.AddressId == message.Message.AddressId))
                        {
                            var address = unit.Addresses.Single(u => u.AddressId == message.Message.AddressId);
                            address.Count += 1;
                        }
                        else
                        {
                            unit.Addresses.Add(new BuildingUnitAddressLinkedDataEventStreamItem
                            {
                                AddressId = message.Message.AddressId,
                                BuildingUnitId = unit.BuildingUnitId,
                                Count = 1,
                                Position = message.Position,
                            });
                        }

                        unit.SetObjectHash();
                        unit.CheckIfRecordCanBePublished();

                        ApplyUnitVersion(unit, message.Message.Provenance.Timestamp, message.EventName);
                    },
                    false,
                    ct);
            });

            When<Envelope<BuildingUnitAddressWasDetached>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        var unit = x.BuildingUnits.Single(u => u.BuildingUnitId == message.Message.From);

                        foreach (var addressId in message.Message.AddressIds)
                        {
                            var addressSyndicationItem = unit.Addresses.SingleOrDefault(u => u.AddressId == addressId);
                            if (addressSyndicationItem != null && addressSyndicationItem.Count > 1)
                                addressSyndicationItem.Count -= 1;
                            else
                                unit.Addresses.Remove(addressSyndicationItem);
                        }

                        unit.SetObjectHash();
                        unit.CheckIfRecordCanBePublished();

                        ApplyUnitVersion(unit, message.Message.Provenance.Timestamp, message.EventName);
                    },
                    false,
                    ct);
            });

            When<Envelope<BuildingUnitBecameComplete>>(async (context, message, ct) =>
            {
                // IGNORE
            });

            When<Envelope<BuildingUnitBecameIncomplete>>(async (context, message, ct) =>
            {
                // IGNORE
            });

            When<Envelope<BuildingUnitPersistentLocalIdWasAssigned>>(async (context, message, ct) =>
            {
                await context.UpdateBuildingUnitPersistentLocalIdentifier(
                    message.Message.BuildingUnitId,
                    message.Message.PersistentLocalId,
                    ct);
            });

            When<Envelope<BuildingUnitPositionWasCorrectedToAppointedByAdministrator>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        var unit = x.BuildingUnits.Single(y => y.BuildingUnitId == message.Message.BuildingUnitId);

                        unit.PositionMethod = BuildingUnitPositionGeometryMethod.AppointedByAdministrator;
                        unit.PointPosition = message.Message.ExtendedWkbGeometry.ToByteArray();

                        unit.SetObjectHash();
                        unit.CheckIfRecordCanBePublished();

                        ApplyUnitVersion(unit, message.Message.Provenance.Timestamp, message.EventName);
                    },
                    false,
                    ct);
            });

            When<Envelope<BuildingUnitPositionWasCorrectedToDerivedFromObject>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        var unit = x.BuildingUnits.Single(y => y.BuildingUnitId == message.Message.BuildingUnitId);

                        unit.PositionMethod = BuildingUnitPositionGeometryMethod.DerivedFromObject;
                        unit.PointPosition = message.Message.ExtendedWkbGeometry.ToByteArray();

                        unit.SetObjectHash();
                        unit.CheckIfRecordCanBePublished();

                        ApplyUnitVersion(unit, message.Message.Provenance.Timestamp, message.EventName);
                    },
                    false,
                    ct);
            });

            When<Envelope<BuildingUnitPositionWasDerivedFromObject>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        var unit = x.BuildingUnits.Single(y => y.BuildingUnitId == message.Message.BuildingUnitId);

                        unit.PositionMethod = BuildingUnitPositionGeometryMethod.DerivedFromObject;
                        unit.PointPosition = message.Message.ExtendedWkbGeometry.ToByteArray();

                        unit.SetObjectHash();
                        unit.CheckIfRecordCanBePublished();

                        ApplyUnitVersion(unit, message.Message.Provenance.Timestamp, message.EventName);
                    },
                    false,
                    ct);
            });

            When<Envelope<BuildingUnitPositionWasAppointedByAdministrator>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        var unit = x.BuildingUnits.Single(y => y.BuildingUnitId == message.Message.BuildingUnitId);

                        unit.PositionMethod = BuildingUnitPositionGeometryMethod.AppointedByAdministrator;
                        unit.PointPosition = message.Message.ExtendedWkbGeometry.ToByteArray();

                        unit.SetObjectHash();
                        unit.CheckIfRecordCanBePublished();

                        ApplyUnitVersion(unit, message.Message.Provenance.Timestamp, message.EventName);
                    },
                    false,
                    ct);
            });

            When<Envelope<BuildingUnitStatusWasRemoved>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        var unit = x.BuildingUnits.Single(y => y.BuildingUnitId == message.Message.BuildingUnitId);

                        unit.Status = null;

                        unit.SetObjectHash();
                        unit.CheckIfRecordCanBePublished();

                        ApplyUnitVersion(unit, message.Message.Provenance.Timestamp, message.EventName);
                    },
                    false,
                    ct);
            });

            When<Envelope<BuildingUnitWasCorrectedToNotRealized>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        var unit = x.BuildingUnits.Single(y => y.BuildingUnitId == message.Message.BuildingUnitId);

                        unit.Status = BuildingUnitStatus.NotRealized;

                        unit.SetObjectHash();
                        unit.CheckIfRecordCanBePublished();

                        ApplyUnitVersion(unit, message.Message.Provenance.Timestamp, message.EventName);
                    },
                    false,
                    ct);
            });

            When<Envelope<BuildingUnitWasCorrectedToPlanned>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        var unit = x.BuildingUnits.Single(y => y.BuildingUnitId == message.Message.BuildingUnitId);

                        unit.Status = BuildingUnitStatus.Planned;

                        unit.SetObjectHash();
                        unit.CheckIfRecordCanBePublished();

                        ApplyUnitVersion(unit, message.Message.Provenance.Timestamp, message.EventName);
                    },
                    false,
                    ct);
            });

            When<Envelope<BuildingUnitWasCorrectedToRealized>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        var unit = x.BuildingUnits.Single(y => y.BuildingUnitId == message.Message.BuildingUnitId);

                        unit.Status = BuildingUnitStatus.Realized;

                        unit.SetObjectHash();
                        unit.CheckIfRecordCanBePublished();

                        ApplyUnitVersion(unit, message.Message.Provenance.Timestamp, message.EventName);
                    },
                    false,
                    ct);
            });

            When<Envelope<BuildingUnitWasCorrectedToRetired>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        var unit = x.BuildingUnits.Single(y => y.BuildingUnitId == message.Message.BuildingUnitId);

                        unit.Status = BuildingUnitStatus.Retired;

                        unit.SetObjectHash();
                        unit.CheckIfRecordCanBePublished();

                        ApplyUnitVersion(unit, message.Message.Provenance.Timestamp, message.EventName);
                    },
                    false,
                    ct);
            });

            When<Envelope<BuildingUnitWasNotRealized>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        var unit = x.BuildingUnits.Single(y => y.BuildingUnitId == message.Message.BuildingUnitId);
                        unit.Status = BuildingUnitStatus.NotRealized;

                        unit.SetObjectHash();
                        unit.CheckIfRecordCanBePublished();

                        ApplyUnitVersion(unit, message.Message.Provenance.Timestamp, message.EventName);
                    },
                    false,
                    ct);
            });

            When<Envelope<BuildingUnitWasNotRealizedByParent>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        var unit = x.BuildingUnits.Single(y => y.BuildingUnitId == message.Message.BuildingUnitId);

                        unit.Status = BuildingUnitStatus.NotRealized;

                        unit.SetObjectHash();
                        unit.CheckIfRecordCanBePublished();

                        ApplyUnitVersion(unit, message.Message.Provenance.Timestamp, message.EventName);
                    },
                    false,
                    ct);
            });

            When<Envelope<BuildingUnitWasPlanned>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        var unit = x.BuildingUnits.Single(y => y.BuildingUnitId == message.Message.BuildingUnitId);

                        unit.Status = BuildingUnitStatus.Planned;

                        unit.SetObjectHash();
                        unit.CheckIfRecordCanBePublished();

                        ApplyUnitVersion(unit, message.Message.Provenance.Timestamp, message.EventName);
                    },
                    false,
                    ct);
            });

            When<Envelope<BuildingUnitWasRealized>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        var unit = x.BuildingUnits.Single(y => y.BuildingUnitId == message.Message.BuildingUnitId);

                        unit.Status = BuildingUnitStatus.Realized;

                        unit.SetObjectHash();
                        unit.CheckIfRecordCanBePublished();

                        ApplyUnitVersion(unit, message.Message.Provenance.Timestamp, message.EventName);
                    },
                    false,
                    ct);
            });

            When<Envelope<BuildingUnitWasRetired>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        var unit = x.BuildingUnits.Single(y => y.BuildingUnitId == message.Message.BuildingUnitId);

                        unit.Status = BuildingUnitStatus.Retired;

                        unit.SetObjectHash();
                        unit.CheckIfRecordCanBePublished();

                        ApplyUnitVersion(unit, message.Message.Provenance.Timestamp, message.EventName);
                    },
                    false,
                    ct);
            });

            When<Envelope<BuildingUnitWasRetiredByParent>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        var unit = x.BuildingUnits.Single(y => y.BuildingUnitId == message.Message.BuildingUnitId);

                        unit.Status = BuildingUnitStatus.Retired;

                        unit.SetObjectHash();
                        unit.CheckIfRecordCanBePublished();

                        ApplyUnitVersion(unit, message.Message.Provenance.Timestamp, message.EventName);
                    },
                    false,
                    ct);
            });

            When<Envelope<BuildingUnitWasReaddressed>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x =>
                    {
                        x.BuildingUnits
                            .Single(y => y.BuildingUnitId == message.Message.BuildingUnitId)
                            .Readdresses
                            .Add(new BuildingUnitReaddressLinkedDataEventStreamItem
                            {
                                Position = message.Position,
                                BuildingUnitId = message.Message.BuildingUnitId,
                                OldAddressId = message.Message.OldAddressId,
                                NewAddressId = message.Message.NewAddressId,
                                ReaddressBeginDate = message.Message.BeginDate
                            });
                    },
                    false,
                    ct);
            });

            When<Envelope<BuildingUnitPersistentLocalIdWasDuplicated>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x => { },
                    false,
                    ct);
            });

            When<Envelope<BuildingUnitPersistentLocalIdWasRemoved>>(async (context, message, ct) =>
            {
                await context.CreateNewBuildingLinkedDataEventStreamItem(
                    message.Message.BuildingId,
                    message,
                    x => { },
                    false,
                    ct);
            });

            #endregion
        }

        private static void ApplyUnitVersion(BuildingUnitLinkedDataEventStreamItem item, Instant generatedAtTime, string eventName)
        {
            item.EventGeneratedAtTime = generatedAtTime;
            item.ChangeType = eventName;
        }

        private static void RetireUnitsByBuilding(
            IEnumerable<BuildingUnitLinkedDataEventStreamItem> buildingUnits,
            ICollection<Guid> buildingUnitIdsToNotRealize,
            ICollection<Guid> buildingUnitIdsToRetire,
            Instant generatedAtTime,
            string eventName
            )
        {
            foreach (var buildingUnitDetailItem in buildingUnits)
            {
                if (buildingUnitIdsToNotRealize.Contains(buildingUnitDetailItem.BuildingUnitId))
                    buildingUnitDetailItem.Status = BuildingUnitStatus.NotRealized;
                else if (buildingUnitIdsToRetire.Contains(buildingUnitDetailItem.BuildingUnitId))
                    buildingUnitDetailItem.Status = BuildingUnitStatus.Retired;

                buildingUnitDetailItem.Addresses.Clear();

                ApplyUnitVersion(buildingUnitDetailItem, generatedAtTime, eventName);
            }
        }
    }
}
