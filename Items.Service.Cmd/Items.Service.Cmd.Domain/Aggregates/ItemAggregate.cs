using CQRS.Core.Domain;
using Items.Service.Common.Events;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Diagnostics;
using System.Xml.Linq;
using System;

namespace Items.Service.Cmd.Domain.Aggregates
{
    public class ItemAggregate : AggregateRoot
    {
        // General Information
        public string? _code;
        public string? _name;
        public string? _foreignName;
        public Guid? _typeId;
        public Guid? _categoryId;
		public Guid? _unitId;
        public Guid? _supplierId;
        public Guid? _taxCodeTypeId;
        public string? _taxCode;
        public string? _notes;
		public decimal _price;
        public decimal _cost;
        public bool _isForSale;
        public bool _isForPurchase;
		public bool _isActive = true;

		public ItemAggregate()
        {
        }

        public ItemAggregate(Guid id, string? code, string? name, string? foreignName, Guid? typeId, Guid? categoryId, Guid? unitId, Guid? supplierId, Guid? taxCodeTypeId, string? taxCode, string? notes, decimal price, decimal cost, bool isForSale, bool isForPurchase, bool isActive)
        {
            RaiseEvent(new ItemCreatedEvent
            {
                Id = id,
				Code = code,
				Name = name,
				ForeignName = foreignName,
				TypeId = typeId,
				CategoryId = categoryId,
				UnitId = unitId,
				SupplierId = supplierId,
				TaxCodeTypeId = taxCodeTypeId,
				TaxCode = taxCode,
				Notes = notes,
				Price = price,
				Cost = cost,
				IsForSale = isForSale,
				IsForPurchase = isForPurchase,
				IsActive = isActive
			});
		}

        public void Apply(ItemCreatedEvent @event)
        {
            _id = @event.Id;
			_code = @event.Code;
			_name = @event.Name;
			_foreignName = @event.ForeignName;
			_typeId = @event.TypeId;
			_categoryId = @event.CategoryId;
			_unitId = @event.UnitId;
			_supplierId = @event.SupplierId;
			_taxCodeTypeId = @event.TaxCodeTypeId;
			_taxCode = @event.TaxCode;
			_notes = @event.Notes;
			_price = @event.Price;
			_cost = @event.Cost;
			_isForSale = @event.IsForSale;
			_isForPurchase = @event.IsForPurchase;
            _isActive = @event.IsActive;

		}
        public void EditItemAggregate(Guid id, string? code, string? name, string? foreignName, Guid? typeId, Guid? categoryId, Guid? unitId, Guid? supplierId, Guid? taxCodeTypeId, string? taxCode, string? notes, decimal price, decimal cost, bool isForSale, bool isForPurchase, bool isActive)
        {
            RaiseEvent(new ItemEditedEvent
            {
				Id = id,
				Code = code,
				Name = name,
				ForeignName = foreignName,
				TypeId = typeId,
				CategoryId = categoryId,
				UnitId = unitId,
				SupplierId = supplierId,
				TaxCodeTypeId = taxCodeTypeId,
				TaxCode = taxCode,
				Notes = notes,
				Price = price,
				Cost = cost,
				IsForSale = isForSale,
				IsForPurchase = isForPurchase,
				IsActive = isActive
			});
        }

        public void Apply(ItemEditedEvent @event)
        {
            _id = @event.Id;
			_code = @event.Code;
			_name = @event.Name;
			_foreignName = @event.ForeignName;
			_typeId = @event.TypeId;
			_categoryId = @event.CategoryId;
			_unitId = @event.UnitId;
			_supplierId = @event.SupplierId;
			_taxCodeTypeId = @event.TaxCodeTypeId;
			_taxCode = @event.TaxCode;
			_notes = @event.Notes;
			_price = @event.Price;
			_cost = @event.Cost;
			_isForSale = @event.IsForSale;
			_isForPurchase = @event.IsForPurchase;
			_isActive = @event.IsActive;
		}

        public void DeleteItem(Guid id)
        {
            RaiseEvent(new ItemDeletedEvent
            {
                Id = id
            });
        }

        public void Apply(ItemDeletedEvent @event)
        {
            _id = @event.Id;
        }

        public void DeleteItemPermanently(Guid id)
        {
            RaiseEvent(new ItemPermanentlyDeletedEvent
            {
                Id = id
            });
        }

        public void Apply(ItemPermanentlyDeletedEvent @event)
        {
            _id = @event.Id;
        }

    }
}
