create table countries
(
	id integer not null
		constraint countries_pk
			primary key,
	country_tag varchar(4)
);

alter table countries owner to service;

create unique index countries_country_tag_uindex
	on countries (country_tag);

create table province
(
	province_id serial not null
		constraint province_pk
			primary key,
	country_id integer
		constraint province_countries_id_fk
			references countries,
	province_name varchar(128)
);

alter table province owner to service;

create index province_country_id_index
	on province (country_id);

create table districts
(
	district_id integer not null
		constraint districts_pk
			primary key,
	province_id integer
		constraint districts_province_province_id_fk
			references province,
	district_name varchar(128)
);

alter table districts owner to service;

create index districts_district_name_index
	on districts (district_name);

create index districts_province_id_index
	on districts (province_id);

create table neighborhoods
(
	neighborhood_id serial not null
		constraint neighborhoods_pk
			primary key,
	district_id integer
		constraint neighborhoods_districts_district_id_fk
			references districts,
	neighborhood_name varchar(128),
	postal_code varchar(16),
	latitude numeric(10,8),
	longitude numeric(11,8)
);

alter table neighborhoods owner to service;

create index neighborhoods_district_id_index
	on neighborhoods (district_id);

create index neighborhoods_longitude_latitude_index
	on neighborhoods (longitude, latitude);

create index neighborhoods_postal_code_index
	on neighborhoods (postal_code);

create table spatial_ref_sys
(
	srid integer not null
		constraint spatial_ref_sys_pkey
			primary key
		constraint spatial_ref_sys_srid_check
			check ((srid > 0) AND (srid <= 998999)),
	auth_name varchar(256),
	auth_srid integer,
	srtext varchar(2048),
	proj4text varchar(2048)
);

alter table spatial_ref_sys owner to service;

grant select on spatial_ref_sys to public;

