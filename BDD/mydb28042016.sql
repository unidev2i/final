-- MySQL dump 10.13  Distrib 5.7.9, for Win64 (x86_64)
--
-- Host: localhost    Database: mydb
-- ------------------------------------------------------
-- Server version	5.7.11-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `classe`
--

DROP TABLE IF EXISTS `classe`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `classe` (
  `idClasse` tinyint(3) unsigned NOT NULL AUTO_INCREMENT,
  `Promotion` varchar(10) NOT NULL,
  `hashClasse` varchar(35) CHARACTER SET utf8 NOT NULL,
  PRIMARY KEY (`idClasse`)
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=latin1 COMMENT='une ligne pour chaque classe';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `classe`
--

LOCK TABLES `classe` WRITE;
/*!40000 ALTER TABLE `classe` DISABLE KEYS */;
INSERT INTO `classe` VALUES (18,'2015','0'),(19,'2017','0');
/*!40000 ALTER TABLE `classe` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `competence`
--

DROP TABLE IF EXISTS `competence`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `competence` (
  `idCompetence` varchar(10) CHARACTER SET utf8 NOT NULL,
  `maxEchelle` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`idCompetence`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `competence`
--

LOCK TABLES `competence` WRITE;
/*!40000 ALTER TABLE `competence` DISABLE KEYS */;
INSERT INTO `competence` VALUES ('CP1.1',0),('CP1.2',0),('CP1.3',0),('CP2.1',0),('CP2.3',0),('CP3.1',0),('CP3.2',0);
/*!40000 ALTER TABLE `competence` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `eleve`
--

DROP TABLE IF EXISTS `eleve`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `eleve` (
  `idEleve` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Nom` varchar(30) NOT NULL COMMENT 'Nom de l''élève',
  `Prenom` varchar(30) NOT NULL COMMENT 'Prénom de l''élève',
  `idClasse` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`idEleve`),
  KEY `idClasse` (`idClasse`),
  CONSTRAINT `fk_classe_eleve` FOREIGN KEY (`idClasse`) REFERENCES `classe` (`idClasse`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8 COMMENT='une ligne pour chaque élève';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `eleve`
--

LOCK TABLES `eleve` WRITE;
/*!40000 ALTER TABLE `eleve` DISABLE KEYS */;
INSERT INTO `eleve` VALUES (19,'VANCAYZEELE','MATTHIEU',18),(20,'DUMORTIER','PAUL',19),(21,'MORAND','MAXENCE',19);
/*!40000 ALTER TABLE `eleve` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `note`
--

DROP TABLE IF EXISTS `note`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `note` (
  `ID` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `idPdf` int(10) unsigned NOT NULL,
  `idCompetence` varchar(10) NOT NULL COMMENT 'ex: cp2.2 ; autonomie ; ...',
  `Note` varchar(10) NOT NULL,
  `maxNote` varchar(10) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `idTp` (`idPdf`),
  KEY `idCompetence` (`idCompetence`),
  CONSTRAINT `fk_note_tp` FOREIGN KEY (`idPdf`) REFERENCES `tp` (`idPdf`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=140 DEFAULT CHARSET=utf8 COMMENT='une ligne pour chaque note';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `note`
--

LOCK TABLES `note` WRITE;
/*!40000 ALTER TABLE `note` DISABLE KEYS */;
INSERT INTO `note` VALUES (113,72,'CP1.1','4 ','9 '),(114,72,'CP2.1','22 ','20 '),(115,72,'CP3.1','2\r\n','2\r'),(116,73,'CP1.1','6 ','4 '),(117,73,'CP1.2','20 ','20 '),(118,73,'CP1.3','3\r\n','3\r'),(119,74,'CP2.1','10 ','4 '),(120,74,'CP2.3','20 ','20 '),(121,74,'CP3.2','2\r\n','2\r'),(122,75,'CP1.1','4 ','9 '),(123,75,'CP2.1','20 ','20 '),(124,75,'CP3.1','2\r\n','2\r'),(125,76,'CP1.1','6 ','4 '),(126,76,'CP1.2','17 ','20 '),(127,76,'CP1.3','2\r\n','3\r'),(128,77,'CP2.1','10 ','4 '),(129,77,'CP2.3','14 ','20 '),(130,77,'CP3.2','1\r\n','2\r'),(131,78,'CP1.1','4 ','9 '),(132,78,'CP2.1','10 ','20 '),(133,78,'CP3.1','1\r\n','2\r'),(134,79,'CP1.1','6 ','4 '),(135,79,'CP1.2','10 ','20 '),(136,79,'CP1.3','1\r\n','3\r'),(137,80,'CP2.1','10 ','4 '),(138,80,'CP2.3','15 ','20 '),(139,80,'CP3.2','2\r\n','2\r');
/*!40000 ALTER TABLE `note` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tp`
--

DROP TABLE IF EXISTS `tp`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tp` (
  `idPdf` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `idTp` varchar(50) NOT NULL COMMENT 'ex: 2 => TP2',
  `idEleve` int(10) unsigned NOT NULL,
  `idcorrecteur` int(10) unsigned NOT NULL,
  `date` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `hashTp` varchar(35) NOT NULL,
  PRIMARY KEY (`idPdf`),
  KEY `idPdf` (`idPdf`),
  KEY `idEleve` (`idEleve`),
  KEY `idcorrecteur` (`idcorrecteur`),
  CONSTRAINT `fk_tp_eleve` FOREIGN KEY (`idEleve`) REFERENCES `eleve` (`idEleve`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=81 DEFAULT CHARSET=utf8 COMMENT='une ligne pour chaque PDF de lu';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tp`
--

LOCK TABLES `tp` WRITE;
/*!40000 ALTER TABLE `tp` DISABLE KEYS */;
INSERT INTO `tp` VALUES (72,'Tp maintenance 1',19,2,'2016-04-28 17:06:41','6b2cf3f3a2ebc25de088e9f761360013'),(73,'Tp maintenance 2',19,2,'2016-04-28 17:06:41','6c91a581192314d8c770390d34b368ac'),(74,'Tp maintenance 3',19,2,'2016-04-28 17:06:41','ac57a3c7ba83d456cf1fce9e644cd09c'),(75,'Tp maintenance 1',20,2,'2016-04-28 17:06:47','a6ca8978a4197ab9b7b8f87fc98a766d'),(76,'Tp maintenance 2',20,2,'2016-04-28 17:06:47','f77ad4268a503a9fd5df6e365f46a6e8'),(77,'Tp maintenance 3',20,2,'2016-04-28 17:06:47','bb1e77bb650983bc195c61cc0cb3c656'),(78,'Tp maintenance 1',21,2,'2016-04-28 17:06:47','e79662896b19f5fbd162be0edbfd812d'),(79,'Tp maintenance 2',21,2,'2016-04-28 17:06:47','67c39692727f84a73d6507dd4b1354c3'),(80,'Tp maintenance 3',21,2,'2016-04-28 17:06:47','85b4a276e54d0ea51292ce62c2844b0b');
/*!40000 ALTER TABLE `tp` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user` (
  `idUser` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Login` varchar(32) NOT NULL,
  `Password` varchar(32) NOT NULL,
  `Admin` tinyint(1) DEFAULT '0' COMMENT 'si 1 => admin',
  PRIMARY KEY (`idUser`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8 COMMENT='une ligne pour chaque utilisateur (prof)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES (1,'Atogue','azerty',1),(2,'a','a',1),(3,'z','z',0),(5,'Pkubiak','root',1);
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2016-04-28 19:14:50
