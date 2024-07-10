CREATE TABLE [user] (
	[id] int NOT NULL,
	[name] nvarchar(max) NOT NULL,
	[email] nvarchar(max) NOT NULL,
	[password] varbinary(max) NOT NULL,
	[country_id] int NOT NULL,
	[state] nvarchar(max) NOT NULL,
	[address] nvarchar(max) NOT NULL,
	[zipcode] nvarchar(max) NOT NULL,
	[cellphone] nvarchar(max) NOT NULL,
	[vatnumber] nvarchar(max) NOT NULL,
	PRIMARY KEY ([id])
);

CREATE TABLE [category] (
	[id] int NOT NULL UNIQUE,
	[category_description_id] int NOT NULL,
	[avatar] varbinary(max) NOT NULL,
	PRIMARY KEY ([id])
);

CREATE TABLE [category_description] (
	[category_id] int NOT NULL UNIQUE,
	[country_language_id] int NOT NULL UNIQUE,
	[description] nvarchar(max) NOT NULL,
	PRIMARY KEY ([category_id], [country_language_id])
);

CREATE TABLE [product] (
	[id] int NOT NULL UNIQUE,
	[name] nvarchar(max) NOT NULL,
	[category_id] int NOT NULL,
	[product_description_id] int NOT NULL,
	[price] float(53) NOT NULL,
	[origin] nvarchar(max) NOT NULL,
	[weight] nvarchar(max) NOT NULL,
	[nutrition_info_id] int NOT NULL,
	PRIMARY KEY ([id])
);

CREATE TABLE [product_description] (
	[product_description_id] int NOT NULL UNIQUE,
	[product_id] int NOT NULL UNIQUE,
	[country_language_id] int NOT NULL UNIQUE,
	[description] nvarchar(max) NOT NULL,
	PRIMARY KEY ([product_id], [country_language_id])
);

CREATE TABLE [nutrition_info] (
	[nutrition_info_id] int NOT NULL UNIQUE,
	[nutrition_info_description] int NOT NULL UNIQUE,
	PRIMARY KEY ([nutrition_info_id], [nutrition_info_description])
);

CREATE TABLE [farmer] (
	[id] int NOT NULL UNIQUE,
	[name] nvarchar(max) NOT NULL,
	[location] nvarchar(max) NOT NULL,
	[picture] varbinary(max) NOT NULL,
	[farmer_info_id] int NOT NULL,
	PRIMARY KEY ([id])
);

CREATE TABLE [product_stock] (
	[product_id] int NOT NULL,
	[stock] int NOT NULL,
	PRIMARY KEY ([product_id], [stock])
);

CREATE TABLE [country] (
	[id] int NOT NULL UNIQUE,
	[iso_alpha_code_2] nvarchar(2) NOT NULL,
	[iso_alpha_code_3] nvarchar(3) NOT NULL,
	[name] nvarchar(max) NOT NULL,
	PRIMARY KEY ([id])
);

CREATE TABLE [farmer_products] (
	[farmer_id] int NOT NULL,
	[product_id] int NOT NULL,
	PRIMARY KEY ([farmer_id], [product_id])
);

CREATE TABLE [product_images] (
	[product_id] int NOT NULL,
	[image] varbinary(max) NOT NULL
);

CREATE TABLE [nutrition_info_description] (
	[id] int NOT NULL UNIQUE,
	[country_language_id] int NOT NULL,
	[description] nvarchar(max) NOT NULL,
	PRIMARY KEY ([id])
);

CREATE TABLE [farmer_info_description] (
	[id] int NOT NULL UNIQUE,
	[country_language_id] int NOT NULL UNIQUE,
	[description] nvarchar(max) NOT NULL,
	PRIMARY KEY ([id], [country_language_id])
);

ALTER TABLE [user] ADD CONSTRAINT [user_fk4] FOREIGN KEY ([country_id]) REFERENCES [country]([id]);
ALTER TABLE [category] ADD CONSTRAINT [category_fk1] FOREIGN KEY ([category_description_id]) REFERENCES [category_description]([id]);
ALTER TABLE [category_description] ADD CONSTRAINT [category_description_fk0] FOREIGN KEY ([category_id]) REFERENCES [category]([id]);

ALTER TABLE [category_description] ADD CONSTRAINT [category_description_fk1] FOREIGN KEY ([country_language_id]) REFERENCES [country]([id]);
ALTER TABLE [product] ADD CONSTRAINT [product_fk2] FOREIGN KEY ([category_id]) REFERENCES [category]([id]);

ALTER TABLE [product] ADD CONSTRAINT [product_fk3] FOREIGN KEY ([product_description_id]) REFERENCES [product_description]([product_description_id]);

ALTER TABLE [product] ADD CONSTRAINT [product_fk7] FOREIGN KEY ([nutrition_info_id]) REFERENCES [nutrition_info]([nutrition_info_id]);
ALTER TABLE [product_description] ADD CONSTRAINT [product_description_fk1] FOREIGN KEY ([product_id]) REFERENCES [product]([id]);

ALTER TABLE [product_description] ADD CONSTRAINT [product_description_fk2] FOREIGN KEY ([country_language_id]) REFERENCES [country]([id]);
ALTER TABLE [nutrition_info] ADD CONSTRAINT [nutrition_info_fk0] FOREIGN KEY ([nutrition_info_id]) REFERENCES [nutrition_info]([id]);

ALTER TABLE [nutrition_info] ADD CONSTRAINT [nutrition_info_fk1] FOREIGN KEY ([nutrition_info_description]) REFERENCES [nutrition_info_description]([id]);
ALTER TABLE [farmer] ADD CONSTRAINT [farmer_fk4] FOREIGN KEY ([farmer_info_id]) REFERENCES [farmer_info_description]([id]);
ALTER TABLE [product_stock] ADD CONSTRAINT [product_stock_fk0] FOREIGN KEY ([product_id]) REFERENCES [product]([id]);

ALTER TABLE [farmer_products] ADD CONSTRAINT [farmer_products_fk0] FOREIGN KEY ([farmer_id]) REFERENCES [farmer]([id]);

ALTER TABLE [farmer_products] ADD CONSTRAINT [farmer_products_fk1] FOREIGN KEY ([product_id]) REFERENCES [product]([id]);
ALTER TABLE [product_images] ADD CONSTRAINT [product_images_fk0] FOREIGN KEY ([product_id]) REFERENCES [product]([id]);
ALTER TABLE [nutrition_info_description] ADD CONSTRAINT [nutrition_info_description_fk1] FOREIGN KEY ([country_language_id]) REFERENCES [country]([id]);
ALTER TABLE [farmer_info_description] ADD CONSTRAINT [farmer_info_description_fk1] FOREIGN KEY ([country_language_id]) REFERENCES [country]([id]);