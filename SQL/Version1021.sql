Insert into Cliente_Empresa (ID_Empresa,ID_Cliente, Predeterminada) Select (Select top 1 ID_Empresa From Empresa order by ID_Empresa), ID_Cliente, 1 From Cliente
Insert into Proveedor_Empresa (ID_Empresa,ID_Proveedor, Predeterminada) Select (Select top 1 ID_Empresa From Empresa order by ID_Empresa), ID_Proveedor, 1 From Proveedor
Insert into Personal_Empresa (ID_Empresa,ID_Personal, Predeterminada) Select (Select top 1 ID_Empresa From Empresa order by ID_Empresa), ID_Personal, 1 From Personal
Update Propuesta Set ID_Empresa=(Select top 1 ID_Empresa From Empresa order by ID_Empresa)
Update Entrada Set ID_Empresa=(Select top 1 ID_Empresa From Empresa order by ID_Empresa)