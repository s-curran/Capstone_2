-- Delete all of the data
DELETE FROM reservation;
DELETE FROM site;
DELETE FROM campground;
DELETE FROM park;


-- Insert a fake Parks
INSERT INTO Park
(name, location, establish_date, area, visitors, description)
VALUES
('Park1', 'Andes Mtn', '1919-01-01', 435, 65, 'blahblahblah'),
('Park2', 'Hondoros', '1919-03-09', 675, 100, 'yooooooooooooooo');


DEclare @lastpark int = @@identity 



-- Insert a fake Campgrounds
INSERT INTO campground
(park_id, name, open_from_mm, open_to_mm, daily_fee)
VALUES
(@lastpark - 1, 'Yogi Bear', 2, 4, 56), 
(@lastpark, 'Smokey Bear', 5, 10, 40);

Declare @lastcampground int = @@identity

-- Insert a fake Sites
INSERT INTO Site
(campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities)
VALUES
(@lastcampground-1, 3, 6, 1, 65, 0), 
(@lastcampground, 2, 7, 0, 56, 1);

Declare @lastsite int = @@identity


-- Assign Projects to reservations
INSERT INTO reservation
(site_id, name, from_date, to_date, create_date)
VALUES
(@lastsite-1, 'res1', '2020-09-08', '2020-09-10', GETDATE()),
(@lastsite, 'res2', '2020-07-15', '2020-08-19', GETDATE());

Declare @lastres int = @@identity


Select @lastpark AS lastpark, @lastcampground AS lastCampground, @lastsite AS lastSite, @lastres AS lastRes
