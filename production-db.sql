create table account
(
	account_id integer default nextval('account_seq'::regclass) not null
		constraint account_pkey
			primary key,
	email varchar(64) not null,
	password varchar(64) not null,
	deleted boolean not null,
	deleted_at date
);

alter table account owner to service;

create table brand
(
	brand_id integer default nextval('brand_seq'::regclass) not null
		constraint brand_pkey
			primary key,
	brand_name varchar(64) not null
);

alter table brand owner to service;

create table category
(
	category_id integer default nextval('category_seq'::regclass) not null
		constraint category_pkey
			primary key,
	category_name varchar(32) not null
);

alter table category owner to service;

create table customer
(
	customer_id integer default nextval('customer_seq'::regclass) not null
		constraint customer_pkey
			primary key,
	account_id integer not null
		constraint customer_account_id_key
			unique
		constraint customer_account_id_fkey
			references account
				on delete cascade,
	first_name varchar(32) not null,
	last_name varchar(32) not null,
	profile_picture_url text not null,
	phone_number varchar(48)
);

alter table customer owner to service;

create table customer_basket
(
	basket_id integer default nextval('customer_basket_seq'::regclass) not null
		constraint customer_basket_pkey
			primary key,
	customer_id integer not null
		constraint customer_basket_customer_id_key
			unique
		constraint customer_basket_customer_id_fkey
			references customer
				on delete cascade
);

alter table customer_basket owner to service;

create table postal_service
(
	service_id integer not null
		constraint postal_service_pkey
			primary key,
	postalservicename varchar(32) not null,
	price double precision not null,
	avg_delivery_day integer
);

alter table postal_service owner to service;

create table customer_order
(
	order_id integer default nextval('customer_order_seq'::regclass) not null
		constraint customer_order_pkey
			primary key,
	customer_id integer
		constraint customer_order_customer_id_fkey
			references customer,
	receiver_first varchar(32) not null,
	receiver_last varchar(32) not null,
	phone_number varchar(32) not null,
	optional_mail varchar(64),
	postal_service_id integer
		constraint customer_order_postal_service_id_fkey
			references postal_service
				on delete set null,
	coupon_discount integer not null,
	order_status varchar(32) not null,
	order_date date not null,
	required_date date not null,
	shipped_date date not null,
	additional_note text,
	total_price double precision not null,
	cargo_price double precision not null
);

alter table customer_order owner to service;

create index idx_customer_order_customer_id
	on customer_order (customer_id);

create table customer_order_address
(
	id integer default nextval('customer_order_address_id_seq'::regclass) not null
		constraint customer_order_address_pkey
			primary key,
	order_id integer not null
		constraint customer_order_address_order_id_key
			unique
		constraint customer_order_address_order_id_fkey
			references customer_order
				on delete cascade,
	country_tag varchar(4) not null,
	province varchar(64) not null,
	district varchar(64) not null,
	neighborhood varchar(64) not null,
	destination_zip_code varchar(12) not null,
	address_text_primary text not null,
	address_text_secondary text
);

alter table customer_order_address owner to service;

create table product
(
	product_id integer default nextval('product_seq'::regclass) not null
		constraint product_pkey
			primary key,
	product_name varchar(128) not null,
	brand_id integer not null
		constraint product_brand_id_fkey
			references brand,
	category_id integer not null
		constraint product_category_id_fkey
			references category,
	sub_type varchar(16) not null,
	product_color varchar(16) not null,
	product_image text not null,
	product_width double precision not null,
	product_height double precision not null,
	product_weight double precision not null,
	box_pieces integer not null,
	model_year integer not null,
	list_price double precision not null,
	discount integer not null,
	description text not null,
	listed smallint not null
);

alter table product owner to service;

create table basket_item
(
	basket_item_id integer not null
		constraint basket_item_pkey
			primary key,
	basket_id integer not null
		constraint basket_item_basket_id_fkey
			references customer_basket
				on delete cascade,
	product_id integer not null
		constraint basket_item_product_id_fkey
			references product
				on delete cascade,
	amount integer not null
);

alter table basket_item owner to service;

create table customer_review
(
	review_id integer default nextval('customer_review_review_id_seq'::regclass) not null
		constraint customer_review_pkey
			primary key,
	customer_id integer not null
		constraint customer_review_customer_id_fkey
			references customer
				on delete cascade,
	customer_comment text not null,
	customer_rating double precision not null,
	product_id integer not null
		constraint customer_review_product_id_fkey
			references product
				on delete cascade,
	reply_review_id integer
);

alter table customer_review owner to service;

create index idx_customer_review_customer_id
	on customer_review (customer_id);

create index idx_customer_review_product_id
	on customer_review (customer_id);

create index idx_product_list_price
	on product (list_price);

create index idx_product_name
	on product (product_name);

create table product_sales_statistic
(
	product_id integer not null
		constraint product_sales_statistic_pkey
			primary key
		constraint product_sales_statistic_product_id_fkey
			references product
				on delete cascade,
	number_sold integer not null,
	product_rating double precision not null,
	number_viewed integer not null,
	date_added date not null
);

alter table product_sales_statistic owner to service;

create table store
(
	store_id integer default nextval('store_seq'::regclass) not null
		constraint store_pkey
			primary key,
	store_name varchar(64) not null,
	phone_number varchar(48) not null,
	email varchar(64) not null
);

alter table store owner to service;

create table customer_order_item
(
	customer_order_item_id integer default nextval('customer_order_item_seq'::regclass) not null
		constraint customer_order_item_pkey
			primary key,
	order_id integer
		constraint customer_order_item_order_id_fkey
			references customer_order
				on delete cascade,
	product_id integer not null
		constraint customer_order_item_product_id_fkey
			references product,
	quantity integer not null,
	list_price double precision not null,
	product_discount integer not null,
	store_id integer
		constraint customer_order_item_store_id_fkey
			references store
				on delete set null
);

alter table customer_order_item owner to service;

create table stock
(
	stock_id integer default nextval('stock_seq'::regclass) not null
		constraint stock_pkey
			primary key,
	store_id integer not null
		constraint stock_store_id_fkey
			references store
				on delete cascade,
	product_id integer not null
		constraint stock_product_id_fkey
			references product
				on delete cascade,
	quantity integer not null
);

alter table stock owner to service;

create index idx_stock_product_id
	on stock (product_id);

create table store_address
(
	store_id integer not null
		constraint store_address_pkey
			primary key
		constraint store_address_store_id_fkey
			references store
				on delete cascade,
	country_tag varchar(4) not null,
	province varchar(64) not null,
	district varchar(64) not null,
	neighborhood varchar(64) not null,
	zip_code varchar(32) not null,
	address_text_primary text not null,
	address_text_secondary text
);

alter table store_address owner to service;

create index idx_store_address_zip_code
	on store_address (zip_code);

create table store_sales_statistic
(
	store_id integer not null
		constraint store_sales_statistic_pkey
			primary key
		constraint store_sales_statistic_store_id_fkey
			references store
				on delete cascade,
	item_sold integer not null
);

alter table store_sales_statistic owner to service;

