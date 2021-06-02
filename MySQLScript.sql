CREATE DATABASE `loyalto`;

CREATE TABLE `loyalto`.`pengguna` (
  `pid` int(10) AUTO_INCREMENT,
  `puser` varchar(50) NOT NULL,
  `ppass` varchar(20) NOT NULL,
  `pstatus` int(2) NOT NULL,
  PRIMARY KEY (`pid`)
);

CREATE TABLE `loyalto`.`toqen` (
  `pid` int(10) NOT NULL,
  `token` varchar(100) NOT NULL,
  `ttime` DATETIME NOT NULL,
  `texpired` DATETIME NOT NULL,
  PRIMARY KEY (`pid`)
);

CREATE TABLE `loyalto`.`valuta` (
  `vid` int(10) AUTO_INCREMENT,
  `vcode` varchar(3) NOT NULL,
  `vname` varchar(20) NOT NULL,
  `vprice` decimal(10,2) NOT NULL,
  PRIMARY KEY (`vid`)
);

INSERT INTO loyalto.pengguna (puser,ppass,pstatus) VALUES('andi','andi12345',1);

