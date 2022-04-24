/*
-- Query: SELECT * FROM blogengine.aspnetroles
LIMIT 0, 1000

-- Date: 2022-04-23 21:50
*/
INSERT INTO `aspnetroles` (`Id`,`Name`,`NormalizedName`,`ConcurrencyStamp`) VALUES ('0df61526-c398-4a08-9e3a-82963e8ee9f5','Writer','WRITER','637863368765145544');
INSERT INTO `aspnetroles` (`Id`,`Name`,`NormalizedName`,`ConcurrencyStamp`) VALUES ('8dae502f-f958-4278-a268-213e1b91ece3','Public','PUBLIC','637863368765099876');
INSERT INTO `aspnetroles` (`Id`,`Name`,`NormalizedName`,`ConcurrencyStamp`) VALUES ('cd0af599-8091-46d9-9a0f-3b4c205ca777','Admin','ADMIN','637863368764628782');
INSERT INTO `aspnetroles` (`Id`,`Name`,`NormalizedName`,`ConcurrencyStamp`) VALUES ('d3ecf2f2-2aa2-46c1-bbb1-78616b51d2bd','Editor','EDITOR','637863368765183149');
/*
-- Query: SELECT * FROM blogengine.aspnetusers
LIMIT 0, 1000

-- Date: 2022-04-23 21:51
*/
INSERT INTO `aspnetusers` (`Id`,`Discriminator`,`CreatedDate`,`UpdatedDate`,`FirstName`,`LastName`,`Birthday`,`Address`,`City`,`State`,`Country`,`PostalCode`,`MobilePhone`,`UserName`,`NormalizedUserName`,`Email`,`NormalizedEmail`,`EmailConfirmed`,`PasswordHash`,`SecurityStamp`,`ConcurrencyStamp`,`PhoneNumber`,`PhoneNumberConfirmed`,`TwoFactorEnabled`,`LockoutEnd`,`LockoutEnabled`,`AccessFailedCount`) VALUES ('2f5855c0-482e-4b49-baeb-c5a847d7ede3','User','2022-04-23 19:00:26.580104','2022-04-23 19:02:59.106987','Writer','User','1900-01-01 00:00:00.000000','.','.','.','.','.','123456','writer@blog.net','WRITER@BLOG.NET','writer@blog.net','WRITER@BLOG.NET',1,'AQAAAAEAACcQAAAAEFcNVZKWdjnoFZxSso+R9K13KaA8NVwuV7MPCRxCeDhgGEWz/loXXqSgKYQKRRnDpA==','WEY6A7CHDZP6J5RRCFQIGDBKOROTIJZ7','39f7725e-abc9-4a73-abbc-3768a5ed7a22','123456',0,0,NULL,1,0);
INSERT INTO `aspnetusers` (`Id`,`Discriminator`,`CreatedDate`,`UpdatedDate`,`FirstName`,`LastName`,`Birthday`,`Address`,`City`,`State`,`Country`,`PostalCode`,`MobilePhone`,`UserName`,`NormalizedUserName`,`Email`,`NormalizedEmail`,`EmailConfirmed`,`PasswordHash`,`SecurityStamp`,`ConcurrencyStamp`,`PhoneNumber`,`PhoneNumberConfirmed`,`TwoFactorEnabled`,`LockoutEnd`,`LockoutEnabled`,`AccessFailedCount`) VALUES ('588caa98-fd31-4e1e-9d03-1b2f4068034d','User','2022-04-23 18:54:36.652288','2022-04-23 19:03:06.671079','Admin','User','1900-01-01 00:00:00.000000','.','.','.','.','.','123456','admin@blog.net','ADMIN@BLOG.NET','admin@blog.net','ADMIN@BLOG.NET',1,'AQAAAAEAACcQAAAAEBGcHgRtEgODfDbMc0gzvCn78lOWSHB/PNfYpimNd8H9Db3mdF7JybjhIscWQ9AGLQ==','IVB4I3X7ZUTRLEUHGCNCFVMUWRXQQN6A','dacc655a-aaea-48f5-9066-b6bb7dd396aa','123456',0,0,NULL,1,0);
INSERT INTO `aspnetusers` (`Id`,`Discriminator`,`CreatedDate`,`UpdatedDate`,`FirstName`,`LastName`,`Birthday`,`Address`,`City`,`State`,`Country`,`PostalCode`,`MobilePhone`,`UserName`,`NormalizedUserName`,`Email`,`NormalizedEmail`,`EmailConfirmed`,`PasswordHash`,`SecurityStamp`,`ConcurrencyStamp`,`PhoneNumber`,`PhoneNumberConfirmed`,`TwoFactorEnabled`,`LockoutEnd`,`LockoutEnabled`,`AccessFailedCount`) VALUES ('704eb72b-b0c6-4248-893d-dd043d0c15e4','User','2022-04-23 19:02:14.840197','2022-04-23 19:02:49.145931','Public','User','1900-01-01 00:00:00.000000','.','.','.','.','.','123456','public@blog.net','PUBLIC@BLOG.NET','public@blog.net','PUBLIC@BLOG.NET',1,'AQAAAAEAACcQAAAAEBnkrb102pa3lIEkRqPcE5eNFbDLYkTw0pAmgiKSY2BAQXXAwD8NME3w0GE9FP64UA==','5T6BGJXBP47BEBHVMSUSAGL2JXVMF4BJ','24873ae4-a168-48ce-af1c-cfeb4178cd43','123456',0,0,NULL,1,0);
INSERT INTO `aspnetusers` (`Id`,`Discriminator`,`CreatedDate`,`UpdatedDate`,`FirstName`,`LastName`,`Birthday`,`Address`,`City`,`State`,`Country`,`PostalCode`,`MobilePhone`,`UserName`,`NormalizedUserName`,`Email`,`NormalizedEmail`,`EmailConfirmed`,`PasswordHash`,`SecurityStamp`,`ConcurrencyStamp`,`PhoneNumber`,`PhoneNumberConfirmed`,`TwoFactorEnabled`,`LockoutEnd`,`LockoutEnabled`,`AccessFailedCount`) VALUES ('7f596b1b-bc08-4b7c-be5f-5fc0fc575273','User','2022-04-23 19:01:21.033634','2022-04-23 19:02:41.462250','Editor','User','1900-01-01 00:00:00.000000','.','.','.','.','.','123456','editor@blog.net','EDITOR@BLOG.NET','editor@blog.net','EDITOR@BLOG.NET',1,'AQAAAAEAACcQAAAAEAsfTpKr+94cklPwm9aoQkvWobbEtMb7cpYqMkjLFVrQ0e8AAV0VT8QrOzivxbiqOg==','WSSXQTTADBRKZOS7KFGHB3FCUG6IVVZY','db362a4a-4203-4bd0-b518-9262ddc95884','123456',0,0,NULL,1,0);
/*
-- Query: SELECT * FROM blogengine.aspnetuserroles
LIMIT 0, 1000

-- Date: 2022-04-23 21:54
*/
INSERT INTO `aspnetuserroles` (`UserId`,`RoleId`) VALUES ('2f5855c0-482e-4b49-baeb-c5a847d7ede3','0df61526-c398-4a08-9e3a-82963e8ee9f5');
INSERT INTO `aspnetuserroles` (`UserId`,`RoleId`) VALUES ('704eb72b-b0c6-4248-893d-dd043d0c15e4','8dae502f-f958-4278-a268-213e1b91ece3');
INSERT INTO `aspnetuserroles` (`UserId`,`RoleId`) VALUES ('588caa98-fd31-4e1e-9d03-1b2f4068034d','cd0af599-8091-46d9-9a0f-3b4c205ca777');
INSERT INTO `aspnetuserroles` (`UserId`,`RoleId`) VALUES ('7f596b1b-bc08-4b7c-be5f-5fc0fc575273','d3ecf2f2-2aa2-46c1-bbb1-78616b51d2bd');
