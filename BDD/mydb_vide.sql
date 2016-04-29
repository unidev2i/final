-- MySqlBackup.NET 2.0.9.3
-- Dump Time: 2016-04-29 22:50:53
-- --------------------------------------
-- Server version 5.6.17 MySQL Community Server (GPL)


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES latin1 */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- 
-- Definition of classe
-- 

DROP TABLE IF EXISTS `classe`;
CREATE TABLE IF NOT EXISTS `classe` (
  `idClasse` tinyint(3) unsigned NOT NULL AUTO_INCREMENT,
  `Promotion` varchar(10) NOT NULL,
  `hashClasse` varchar(35) CHARACTER SET utf8 NOT NULL,
  PRIMARY KEY (`idClasse`)
) ENGINE=InnoDB AUTO_INCREMENT=38 DEFAULT CHARSET=latin1 COMMENT='une ligne pour chaque classe';

-- 
-- Dumping data for table classe
-- 

/*!40000 ALTER TABLE `classe` DISABLE KEYS */;

/*!40000 ALTER TABLE `classe` ENABLE KEYS */;

-- 
-- Definition of competence
-- 

DROP TABLE IF EXISTS `competence`;
CREATE TABLE IF NOT EXISTS `competence` (
  `idCompetence` varchar(10) CHARACTER SET utf8 NOT NULL,
  `maxEchelle` int(10) unsigned NOT NULL DEFAULT '20',
  PRIMARY KEY (`idCompetence`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- 
-- Dumping data for table competence
-- 

/*!40000 ALTER TABLE `competence` DISABLE KEYS */;

/*!40000 ALTER TABLE `competence` ENABLE KEYS */;

-- 
-- Definition of eleve
-- 

DROP TABLE IF EXISTS `eleve`;
CREATE TABLE IF NOT EXISTS `eleve` (
  `idEleve` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Nom` varchar(30) NOT NULL COMMENT 'Nom de l''élève',
  `Prenom` varchar(30) NOT NULL COMMENT 'Prénom de l''élève',
  `idClasse` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`idEleve`),
  KEY `idClasse` (`idClasse`),
  CONSTRAINT `fk_classe_eleve` FOREIGN KEY (`idClasse`) REFERENCES `classe` (`idClasse`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8 COMMENT='une ligne pour chaque élève';

-- 
-- Dumping data for table eleve
-- 

/*!40000 ALTER TABLE `eleve` DISABLE KEYS */;

/*!40000 ALTER TABLE `eleve` ENABLE KEYS */;

-- 
-- Definition of note
-- 

DROP TABLE IF EXISTS `note`;
CREATE TABLE IF NOT EXISTS `note` (
  `ID` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `idPdf` int(10) unsigned NOT NULL,
  `idCompetence` varchar(10) NOT NULL COMMENT 'ex: cp2.2 ; autonomie ; ...',
  `Note` varchar(10) NOT NULL,
  `maxNote` varchar(10) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `idTp` (`idPdf`),
  KEY `idCompetence` (`idCompetence`),
  CONSTRAINT `fk_note_tp` FOREIGN KEY (`idPdf`) REFERENCES `tp` (`idPdf`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=106 DEFAULT CHARSET=utf8 COMMENT='une ligne pour chaque note';

-- 
-- Dumping data for table note
-- 

/*!40000 ALTER TABLE `note` DISABLE KEYS */;

/*!40000 ALTER TABLE `note` ENABLE KEYS */;

-- 
-- Definition of tp
-- 

DROP TABLE IF EXISTS `tp`;
CREATE TABLE IF NOT EXISTS `tp` (
  `idPdf` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `idTp` varchar(200) NOT NULL COMMENT 'ex: 2 => TP2',
  `idEleve` int(10) unsigned NOT NULL,
  `idcorrecteur` int(10) unsigned NOT NULL,
  `date` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `hashTp` varchar(35) NOT NULL,
  PRIMARY KEY (`idPdf`),
  KEY `idPdf` (`idPdf`),
  KEY `idEleve` (`idEleve`),
  KEY `idcorrecteur` (`idcorrecteur`),
  CONSTRAINT `fk_tp_eleve` FOREIGN KEY (`idEleve`) REFERENCES `eleve` (`idEleve`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8 COMMENT='une ligne pour chaque PDF de lu';

-- 
-- Dumping data for table tp
-- 

/*!40000 ALTER TABLE `tp` DISABLE KEYS */;

/*!40000 ALTER TABLE `tp` ENABLE KEYS */;

-- 
-- Definition of user
-- 

DROP TABLE IF EXISTS `user`;
CREATE TABLE IF NOT EXISTS `user` (
  `idUser` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Login` varchar(32) NOT NULL,
  `Password` varchar(32) NOT NULL,
  `Admin` tinyint(1) DEFAULT '0' COMMENT 'si 1 => admin',
  PRIMARY KEY (`idUser`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COMMENT='une ligne pour chaque utilisateur (prof)';

-- 
-- Dumping data for table user
-- 

/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user`(`idUser`,`Login`,`Password`,`Admin`) VALUES
(2,'a','a',1);
/*!40000 ALTER TABLE `user` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2016-04-29 22:50:53
-- Total time: 0:0:0:0:71 (d:h:m:s:ms)
