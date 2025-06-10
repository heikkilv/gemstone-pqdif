﻿//******************************************************************************************************
//  DataSourceRecord.cs - Gbtc
//
//  Copyright © 2012, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  05/03/2012 - Stephen C. Wills, Grid Protection Alliance
//       Generated original version of source code.
//  12/17/2012 - Starlynn Danyelle Gilliam
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Gemstone.PQDIF.Physical;

namespace Gemstone.PQDIF.Logical
{
    /// <summary>
    /// Types of data sources.
    /// </summary>
    public static class DataSourceType
    {
        /// <summary>
        /// The ID for data source type Measure.
        /// </summary>
        public static Guid Measure { get; } = new("e6b51730-f747-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// The ID for data source type Manual.
        /// </summary>
        public static Guid Manual { get; } = new("e6b51731-f747-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// The ID for data source type Simulate.
        /// </summary>
        public static Guid Simulate { get; } = new("e6b51732-f747-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// The ID for data source type Benchmark.
        /// </summary>
        public static Guid Benchmark { get; } = new("e6b51733-f747-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// The ID for data source type Debug.
        /// </summary>
        public static Guid Debug { get; } = new("e6b51734-f747-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Gets information about the data source type identified by the given ID.
        /// </summary>
        /// <param name="dataSourceTypeID">Globally unique identifier for the data source type.</param>
        /// <returns>The information about the data source type.</returns>
        public static Identifier? GetInfo(Guid dataSourceTypeID) =>
            DataSourceTypeLookup.TryGetValue(dataSourceTypeID, out Identifier? identifier) ? identifier : null;

        /// <summary>
        /// Converts the given data source type ID to a string containing the name of the data source type.
        /// </summary>
        /// <param name="dataSourceTypeID">The ID of the data source type to be converted to a string.</param>
        /// <returns>A string containing the name of the data source type with the given ID.</returns>
        public static string ToString(Guid dataSourceTypeID) =>
            GetInfo(dataSourceTypeID)?.Name ?? dataSourceTypeID.ToString();

        private static Dictionary<Guid, Identifier> DataSourceTypeLookup
        {
            get
            {
                Tag? dataSourceTypeTag = Tag.GetTag(DataSourceRecord.DataSourceTypeIDTag);

                if (s_dataSourceTypeTag != dataSourceTypeTag)
                {
                    s_dataSourceTypeTag = dataSourceTypeTag;
                    s_dataSourceTypeLookup = dataSourceTypeTag?.ValidIdentifiers.ToDictionary(id => Guid.Parse(id.Value));
                }

                return s_dataSourceTypeLookup ?? new Dictionary<Guid, Identifier>();
            }
        }

        private static Tag? s_dataSourceTypeTag;
        private static Dictionary<Guid, Identifier>? s_dataSourceTypeLookup;
    }

    /// <summary>
    /// Represents a data source record in a PQDIF file. The data source
    /// record contains information about the source of the data in an
    /// <see cref="ObservationRecord"/>.
    /// </summary>
    public class DataSourceRecord
    {
        #region [ Members ]

        // Fields
        private readonly Record m_physicalRecord;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="DataSourceRecord"/> class.
        /// </summary>
        /// <param name="physicalRecord">The physical structure of the data source record.</param>
        private DataSourceRecord(Record physicalRecord)
        {
            m_physicalRecord = physicalRecord;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the physical structure of the data source record.
        /// </summary>
        public Record PhysicalRecord
        {
            get
            {
                return m_physicalRecord;
            }
        }

        /// <summary>
        /// Gets or sets the ID of the type of the data source.
        /// </summary>
        /// <exception cref="InvalidDataException">DataSourceTypeID element not found in data source record.</exception>
        public Guid DataSourceTypeID
        {
            get
            {
                ScalarElement dataSourceTypeIDElement = m_physicalRecord.Body.Collection.GetScalarByTag(DataSourceTypeIDTag)
                    ?? throw new InvalidDataException("DataSourceTypeID element not found in data source record.");

                return dataSourceTypeIDElement.GetGuid();
            }
            set
            {
                CollectionElement collectionElement = m_physicalRecord.Body.Collection;
                ScalarElement dataSourceTypeIDElement = collectionElement.GetOrAddScalar(DataSourceTypeIDTag);
                dataSourceTypeIDElement.TypeOfValue = PhysicalType.Guid;
                dataSourceTypeIDElement.SetGuid(value);
            }
        }

        /// <summary>
        /// Gets or sets the ID of the vendor of the data source.
        /// </summary>
        public Guid VendorID
        {
            get
            {
                CollectionElement collectionElement = m_physicalRecord.Body.Collection;
                ScalarElement? vendorIDElement = collectionElement.GetScalarByTag(VendorIDTag);

                if (vendorIDElement is null)
                    return Vendor.None;

                return vendorIDElement.GetGuid();
            }
            set
            {
                CollectionElement collectionElement = m_physicalRecord.Body.Collection;
                ScalarElement vendorIDElement = collectionElement.GetOrAddScalar(VendorIDTag);
                vendorIDElement.TypeOfValue = PhysicalType.Guid;
                vendorIDElement.SetGuid(value);
            }
        }

        /// <summary>
        /// Gets or sets the ID of the equipment.
        /// </summary>
        public Guid EquipmentID
        {
            get
            {
                CollectionElement collectionElement = m_physicalRecord.Body.Collection;
                ScalarElement? equipmentIDElement = collectionElement.GetScalarByTag(EquipmentIDTag);

                if (equipmentIDElement is null)
                    return Guid.Empty;

                return equipmentIDElement.GetGuid();
            }
            set
            {
                CollectionElement collectionElement = m_physicalRecord.Body.Collection;
                ScalarElement equipmentIDElement = collectionElement.GetOrAddScalar(EquipmentIDTag);
                equipmentIDElement.TypeOfValue = PhysicalType.Guid;
                equipmentIDElement.SetGuid(value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the data source.
        /// </summary>
        /// <exception cref="InvalidDataException">DataSourceName element not found in data source record.</exception>
        public string DataSourceName
        {
            get
            {
                CollectionElement collectionElement = m_physicalRecord.Body.Collection;

                VectorElement dataSourceNameElement = collectionElement.GetVectorByTag(DataSourceNameTag)
                    ?? throw new InvalidDataException("DataSourceName element not found in data source record.");

                return Encoding.ASCII.GetString(dataSourceNameElement.GetValues()).Trim((char)0);
            }
            set
            {
                CollectionElement collectionElement = m_physicalRecord.Body.Collection;
                byte[] bytes = Encoding.ASCII.GetBytes(value + (char)0);
                collectionElement.AddOrUpdateVector(DataSourceNameTag, PhysicalType.Char1, bytes);
            }
        }

        /// <summary>
        /// Gets or sets the name of the data source Owner.
        /// </summary>
        /// <exception cref="InvalidDataException">DataSourceOwner element not found in data source record.</exception>
        public string DataSourceOwner
        {
            get
            {
                CollectionElement collectionElement = m_physicalRecord.Body.Collection;

                VectorElement dataSourceNameElement = collectionElement.GetVectorByTag(DataSourceOwnerTag)
                    ?? throw new InvalidDataException("DataSourceOwner element not found in data source record.");

                return Encoding.ASCII.GetString(dataSourceNameElement.GetValues()).Trim((char)0);
            }
            set
            {
                CollectionElement collectionElement = m_physicalRecord.Body.Collection;
                byte[] bytes = Encoding.ASCII.GetBytes(value + (char)0);
                collectionElement.AddOrUpdateVector(DataSourceOwnerTag, PhysicalType.Char1, bytes);
            }
        }

        /// <summary>
        /// Gets or sets the name of the data source Location.
        /// </summary>
        /// <exception cref="InvalidDataException">DataSourceLocation element not found in data source record.</exception>
        public string DataSourceLocation
        {
            get
            {
                CollectionElement collectionElement = m_physicalRecord.Body.Collection;

                VectorElement dataSourceNameElement = collectionElement.GetVectorByTag(DataSourceLocationTag)
                    ?? throw new InvalidDataException("DataSourceLocation element not found in data source record.");

                return Encoding.ASCII.GetString(dataSourceNameElement.GetValues()).Trim((char)0);
            }
            set
            {
                CollectionElement collectionElement = m_physicalRecord.Body.Collection;
                byte[] bytes = Encoding.ASCII.GetBytes(value + (char)0);
                collectionElement.AddOrUpdateVector(DataSourceLocationTag, PhysicalType.Char1, bytes);
            }
        }

        /// <summary>
        /// Gets or sets the longitude at which the data source is located.
        /// </summary>
        public uint Longitude
        {
            get
            {
                CollectionElement collectionElement = m_physicalRecord.Body.Collection;
                VectorElement? dataSourceCoordinatesElement = collectionElement.GetVectorByTag(DataSourceCoordinatesTag);

                if (dataSourceCoordinatesElement is null)
                    return uint.MaxValue;

                return dataSourceCoordinatesElement.GetUInt4(0);
            }
            set
            {
                CollectionElement collectionElement = m_physicalRecord.Body.Collection;
                VectorElement dataSourceCoordinatesElement = collectionElement.GetOrAddVector(DataSourceCoordinatesTag);
                dataSourceCoordinatesElement.TypeOfValue = PhysicalType.UnsignedInteger4;
                dataSourceCoordinatesElement.Size = 2;
                dataSourceCoordinatesElement.SetUInt4(0, value);
            }
        }

        /// <summary>
        /// Gets or sets the latitude at which the device is located.
        /// </summary>
        public uint Latitude
        {
            get
            {
                CollectionElement collectionElement = m_physicalRecord.Body.Collection;
                VectorElement? dataSourceCoordinatesElement = collectionElement.GetVectorByTag(DataSourceCoordinatesTag);

                if (dataSourceCoordinatesElement is null)
                    return uint.MaxValue;

                return dataSourceCoordinatesElement.GetUInt4(1);
            }
            set
            {
                CollectionElement collectionElement = m_physicalRecord.Body.Collection;
                VectorElement dataSourceCoordinatesElement = collectionElement.GetOrAddVector(DataSourceCoordinatesTag);
                dataSourceCoordinatesElement.TypeOfValue = PhysicalType.UnsignedInteger4;
                dataSourceCoordinatesElement.Size = 2;
                dataSourceCoordinatesElement.SetUInt4(1, value);
            }
        }

        /// <summary>
        /// Gets the definitions for the channels defined in the data source.
        /// </summary>
        /// <exception cref="InvalidDataException">ChannelDefinitions element not found in data source record.</exception>
        public IList<ChannelDefinition> ChannelDefinitions
        {
            get
            {
                CollectionElement channelDefinitionsElement = m_physicalRecord.Body.Collection.GetCollectionByTag(ChannelDefinitionsTag)
                    ?? throw new InvalidDataException("ChannelDefinitions element not found in data source record.");

                return channelDefinitionsElement
                    .GetElementsByTag(OneChannelDefinitionTag)
                    .Cast<CollectionElement>()
                    .Select(collection => new ChannelDefinition(collection, this))
                    .ToList();
            }
        }

        /// <summary>
        /// Gets or sets the time that this data source record became effective.
        /// </summary>
        /// <exception cref="InvalidDataException">Effective element not found in data source record.</exception>
        public DateTime Effective
        {
            get
            {
                ScalarElement effectiveElement = m_physicalRecord.Body.Collection.GetScalarByTag(EffectiveTag)
                    ?? throw new InvalidDataException("Effective element not found in data source record.");

                return effectiveElement.GetTimestamp();
            }
            set
            {
                CollectionElement collectionElement = m_physicalRecord.Body.Collection;
                ScalarElement effectiveElement = collectionElement.GetOrAddScalar(EffectiveTag);
                effectiveElement.TypeOfValue = PhysicalType.Timestamp;
                effectiveElement.SetTimestamp(value);
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Adds a new channel definition to the collection
        /// of channel definitions in this data source record.
        /// </summary>
        /// <returns>New channel definition.</returns>
        public ChannelDefinition AddNewChannelDefinition()
        {
            CollectionElement channelDefinitionElement = new() { TagOfElement = OneChannelDefinitionTag };
            ChannelDefinition channelDefinition = new(channelDefinitionElement, this);

            channelDefinition.Phase = Phase.None;
            channelDefinition.QuantityMeasured = QuantityMeasured.None;
            channelDefinitionElement.AddElement(new CollectionElement { TagOfElement = ChannelDefinition.SeriesDefinitionsTag });

            CollectionElement? channelDefinitionsElement = m_physicalRecord.Body.Collection.GetCollectionByTag(ChannelDefinitionsTag);

            if (channelDefinitionsElement is null)
            {
                channelDefinitionsElement = new CollectionElement { TagOfElement = ChannelDefinitionsTag };
                m_physicalRecord.Body.Collection.AddElement(channelDefinitionsElement);
            }

            channelDefinitionsElement.AddElement(channelDefinitionElement);

            return channelDefinition;
        }

        /// <summary>
        /// Removes the given channel definition from the collection of channel definitions.
        /// </summary>
        /// <param name="channelDefinition">The channel definition to be removed.</param>
        public void Remove(ChannelDefinition channelDefinition)
        {
            CollectionElement? channelDefinitionsElement = m_physicalRecord.Body.Collection.GetCollectionByTag(ChannelDefinitionsTag);

            if (channelDefinitionsElement is null)
                return;

            List<CollectionElement> channelDefinitionElements = channelDefinitionsElement.GetElementsByTag(OneChannelDefinitionTag).Cast<CollectionElement>().ToList();

            foreach (CollectionElement channelDefinitionElement in channelDefinitionElements)
            {
                ChannelDefinition definition = new(channelDefinitionElement, this);

                if (Equals(channelDefinition, definition))
                    channelDefinitionsElement.RemoveElement(channelDefinitionElement);
            }
        }

        /// <summary>
        /// Removes the element identified by the given tag from the record.
        /// </summary>
        /// <param name="tag">The tag of the element to be removed.</param>
        public void RemoveElement(Guid tag) =>
            m_physicalRecord.Body.Collection.RemoveElementsByTag(tag);

        #endregion

        #region [ Static ]

        // Static Fields

        /// <summary>
        /// Tag that identifies the data source type.
        /// </summary>
        public static Guid DataSourceTypeIDTag { get; } = new("b48d8581-f5f5-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the vendor ID.
        /// </summary>
        public static Guid VendorIDTag { get; } = new("b48d8582-f5f5-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the equipment ID.
        /// </summary>
        public static Guid EquipmentIDTag { get; } = new("b48d8583-f5f5-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the data source name.
        /// </summary>
        public static Guid DataSourceNameTag { get; } = new("b48d8587-f5f5-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the data source owner.
        /// </summary>
        public static Guid DataSourceOwnerTag { get; } = new("b48d8588-f5f5-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the data source owner.
        /// </summary>
        public static Guid DataSourceLocationTag { get; } = new("b48d8589-f5f5-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the physical location of the data source.
        /// </summary>
        public static Guid DataSourceCoordinatesTag { get; } = new("b48d858b-f5f5-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the channel definitions collection.
        /// </summary>
        public static Guid ChannelDefinitionsTag { get; } = new("b48d858d-f5f5-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the a single channel definition in the collection.
        /// </summary>
        public static Guid OneChannelDefinitionTag { get; } = new("b48d858e-f5f5-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the time that the data source record becomes effective.
        /// </summary>
        public static Guid EffectiveTag { get; } = new("62f28183-f9c4-11cf-9d89-0080c72e70a3");

        // Static Methods

        /// <summary>
        /// Creates a new data source record from scratch.
        /// </summary>
        /// <param name="dataSourceName">The name of the data source to be created.</param>
        /// <returns>The new data source record.</returns>
        public static DataSourceRecord CreateDataSourceRecord(string dataSourceName)
        {
            Guid recordTypeTag = Record.GetTypeAsTag(RecordType.DataSource);
            Record physicalRecord = new(recordTypeTag);
            DataSourceRecord dataSourceRecord = new(physicalRecord);

            DateTime now = DateTime.UtcNow;
            dataSourceRecord.DataSourceTypeID = DataSourceType.Simulate;
            dataSourceRecord.DataSourceName = dataSourceName;
            dataSourceRecord.Effective = now;

            CollectionElement bodyElement = physicalRecord.Body.Collection;
            bodyElement.AddElement(new CollectionElement { TagOfElement = ChannelDefinitionsTag });

            return dataSourceRecord;
        }

        /// <summary>
        /// Creates a new data source record from the given physical record
        /// if the physical record is of type data source. Returns null if
        /// it is not.
        /// </summary>
        /// <param name="physicalRecord">The physical record used to create the data source record.</param>
        /// <returns>The new data source record, or null if the physical record does not define a data source record.</returns>
        public static DataSourceRecord? CreateDataSourceRecord(Record physicalRecord)
        {
            bool isValidDataSourceRecord = physicalRecord.Header.TypeOfRecord == RecordType.DataSource;
            return isValidDataSourceRecord ? new DataSourceRecord(physicalRecord) : null;
        }

        #endregion
    }
}
