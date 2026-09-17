
create database nomina;

create table departments (
  -- UUID generado por la base si la app no lo manda
  id uuid primary key default gen_random_uuid(),

  -- codigo corto del area: RRHH, TI, VENTAS
  code varchar(10) not null unique,

  name varchar(60) not null unique,
  description text,
  location varchar(80),

  -- numeric y no float: con plata el redondeo
  -- binario da problemas
  budget numeric(14,2),

  phone varchar(20),
  email varchar(120),

  -- borrado logico: los departamentos disueltos
  -- se desactivan, no se eliminan
  is_active boolean not null default true,

  -- timestamptz guarda la zona horaria
  created_at timestamptz not null default now(),

  -- queda null hasta la primera edicion
  updated_at timestamptz
);

insert into departments
  (code, name, description, location, budget, phone, email)
values
  ('GH', 'Gestión Humana',
   'Selección, contratación y bienestar del personal',
   'Bogotá', 420000000.00,
   '601 7458920', 'gestionhumana@empresa.com.co'),

  ('TI', 'Tecnología',
   'Desarrollo de software y soporte de infraestructura',
   'Medellín', 980000000.00,
   '604 3216548', 'tecnologia@empresa.com.co'),

  ('CON', 'Contabilidad',
   'Nómina, impuestos y estados financieros',
   'Bogotá', 310000000.00,
   '601 7458935', 'contabilidad@empresa.com.co'),

  ('COM', 'Comercial',
   'Ventas, licitaciones y relación con clientes',
   'Barranquilla', 650000000.00,
   '605 3854120', 'comercial@empresa.com.co'),

  ('LOG', 'Logística',
   'Almacén, despachos y transporte',
   'Cali', 480000000.00,
   '602 4471290', 'logistica@empresa.com.co'),

  ('MER', 'Mercadeo',
   'Campañas, marca y comunicaciones',
   'Bogotá', null,
   '601 7458941', 'mercadeo@empresa.com.co');


   create table employees (
  id uuid primary key default gen_random_uuid(),

  -- documento de identidad, unico por persona
  document varchar(15) not null unique,
  first_name varchar(60) not null,
  last_name varchar(60) not null,

  email varchar(120) not null unique,
  phone varchar(20),

  -- cargo del empleado dentro del area
  position varchar(80) not null,
  salary numeric(12,2) not null,
  hire_date date not null,

  -- aqui vive la relacion uno a muchos:
  -- un departamento tiene muchos empleados
  department_id uuid not null
    references departments(id)
    on delete restrict,

  is_active boolean not null default true,
  created_at timestamptz not null default now(),
  updated_at timestamptz
);

-- sin este indice, filtrar empleados por
-- departamento hace scan de toda la tabla
create index ix_employees_department
  on employees(department_id);



  insert into employees
  (document, first_name, last_name, email, phone,
   position, salary, hire_date, department_id)
values
  ('1020458796', 'Andrés', 'Restrepo Gómez',
   'arestrepo@empresa.com.co', '310 4589620',
   'Director de Tecnología', 14500000.00, '2019-03-11',
   (select id from departments where code = 'TI')),

  ('1032658974', 'Laura', 'Mendoza Pérez',
   'lmendoza@empresa.com.co', '315 7896541',
   'Desarrolladora Backend', 7800000.00, '2022-08-01',
   (select id from departments where code = 'TI')),

  ('1018457963', 'Camilo', 'Osorio Vargas',
   'cosorio@empresa.com.co', '320 1478523',
   'Ingeniero de Soporte', 4200000.00, '2023-01-16',
   (select id from departments where code = 'TI')),

  ('52478963', 'Diana', 'Cárdenas Ruiz',
   'dcardenas@empresa.com.co', '311 2589647',
   'Jefe de Gestión Humana', 9600000.00, '2018-06-04',
   (select id from departments where code = 'GH')),

  ('1090785412', 'Sebastián', 'Quintero Lozano',
   'squintero@empresa.com.co', null,
   'Analista de Selección', 3900000.00, '2024-02-19',
   (select id from departments where code = 'GH')),

  ('79854126', 'Jorge', 'Pineda Acosta',
   'jpineda@empresa.com.co', '317 4521896',
   'Contador General', 8200000.00, '2017-09-25',
   (select id from departments where code = 'CON')),

  ('1045789632', 'Marcela', 'Ariza Fontalvo',
   'mariza@empresa.com.co', '301 7412589',
   'Auxiliar Contable', 2800000.00, '2023-07-03',
   (select id from departments where code = 'CON')),

  ('1082457963', 'Kevin', 'De la Hoz Movilla',
   'kdelahoz@empresa.com.co', '316 8523697',
   'Ejecutivo Comercial', 5400000.00, '2021-11-08',
   (select id from departments where code = 'COM')),

  ('1140789456', 'Yuranis', 'Barrios Charris',
   'ybarrios@empresa.com.co', '304 1597534',
   'Gerente Comercial', 11200000.00, '2016-04-18',
   (select id from departments where code = 'COM')),

  ('1144785236', 'Édgar', 'Valencia Muñoz',
   'evalencia@empresa.com.co', '318 3691478',
   'Coordinador de Bodega', 4600000.00, '2020-10-05',
   (select id from departments where code = 'LOG')),

  ('94587412', 'Hernán', 'Caicedo Mina',
   'hcaicedo@empresa.com.co', '312 7539514',
   'Conductor de Despachos', 2200000.00, '2022-05-23',
   (select id from departments where code = 'LOG')),

  ('1026548793', 'Natalia', 'Buitrago Salcedo',
   'nbuitrago@empresa.com.co', '319 8527413',
   'Community Manager', 3600000.00, '2024-09-02',
   (select id from departments where code = 'MER'));