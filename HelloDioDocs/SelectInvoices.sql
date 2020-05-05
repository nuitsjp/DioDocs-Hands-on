use AdventureWorks;

select 
	SalesORderHeader.SalesOrderID,
	GETDATE() as InvitationDay,
	DATEADD(DAY, 30, GETDATE()) as DueDate,
	Address.PostalCode,
	ISNULL(StateProvince.Name, '') + ' '  + ISNULL(Address.City, '') + ' ' + ISNULL(Address.AddressLine1, '') + ' ' + ISNULL(Address.AddressLine2, '') as CustomerAddress,
	Store.Name as StoreName,
	Person.FirstName + ' ' + Person.LastName as CustomerPerson,
	SalesOrderHeader.SubTotal,
	SalesOrderHeader.TaxAmt,
	SalesOrderHeader.Freight,
	SalesOrderHeader.TotalDue,
	Product.Name as ProductName,
	SalesOrderDetail.UnitPrice,
	SalesOrderDetail.OrderQty,
	SalesOrderDetail.UnitPriceDiscount,
	SalesOrderDetail.LineTotal
from
	Sales.SalesOrderHeader
	inner join Person.Address
		on	SalesOrderHeader.BillToAddressID = Address.AddressID
	inner join Person.StateProvince
		on	Address.StateProvinceID = StateProvince.StateProvinceID
	inner join Sales.Customer
		on	Sales.SalesOrderHeader.CustomerID = Customer.CustomerID
	inner join Sales.Store
		on	Customer.StoreID = Store.BusinessEntityID
	inner join Person.Person
		on	Customer.PersonID = Person.BusinessEntityID
	inner join Sales.SalesOrderDetail
		on	SalesOrderHeader.SalesOrderID = SalesOrderDetail.SalesOrderID
	inner join Production.Product
		on	SalesOrderDetail.ProductID = Product.ProductID
where
	SalesOrderHeader.SalesOrderID <= 43661