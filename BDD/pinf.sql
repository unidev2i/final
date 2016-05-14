-- phpMyAdmin SQL Dump
-- version 4.1.14
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: May 14, 2016 at 12:56 PM
-- Server version: 5.6.17
-- PHP Version: 5.5.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `pinf`
--

-- --------------------------------------------------------

--
-- Table structure for table `classe`
--

CREATE TABLE IF NOT EXISTS `classe` (
  `idClasse` tinyint(3) unsigned NOT NULL AUTO_INCREMENT,
  `Promotion` varchar(10) NOT NULL,
  `hashClasse` varchar(35) CHARACTER SET utf8 NOT NULL,
  PRIMARY KEY (`idClasse`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 COMMENT='une ligne pour chaque classe' AUTO_INCREMENT=7 ;

--
-- Dumping data for table `classe`
--

INSERT INTO `classe` (`idClasse`, `Promotion`, `hashClasse`) VALUES
(5, '2016', '0'),
(6, '2017', '0');

-- --------------------------------------------------------

--
-- Table structure for table `competence`
--

CREATE TABLE IF NOT EXISTS `competence` (
  `maxEchelle` int(10) unsigned NOT NULL DEFAULT '20',
  `idClasse` int(11) NOT NULL,
  `idCompetence` varchar(10) NOT NULL,
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=10 ;

--
-- Dumping data for table `competence`
--

INSERT INTO `competence` (`maxEchelle`, `idClasse`, `idCompetence`, `ID`) VALUES
(20, 0, 'C1.1', 1),
(20, 0, '', 2),
(20, 0, '', 3),
(20, 0, '', 4),
(20, 0, '', 5),
(20, 0, '', 6),
(20, 0, '', 7),
(20, 0, '', 8),
(20, 0, '', 9);

-- --------------------------------------------------------

--
-- Table structure for table `eleve`
--

CREATE TABLE IF NOT EXISTS `eleve` (
  `idEleve` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Nom` varchar(30) NOT NULL COMMENT 'Nom de l''élève',
  `Prenom` varchar(30) NOT NULL COMMENT 'Prénom de l''élève',
  `idClasse` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`idEleve`),
  KEY `idClasse` (`idClasse`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COMMENT='une ligne pour chaque élève' AUTO_INCREMENT=10 ;

--
-- Dumping data for table `eleve`
--

INSERT INTO `eleve` (`idEleve`, `Nom`, `Prenom`, `idClasse`) VALUES
(7, 'DUMORTIER', 'PAUL', 5),
(8, 'MORAND', 'MAXENCE', 6),
(9, 'VANCAYZEELE', 'MATTHIEU', 6);

-- --------------------------------------------------------

--
-- Table structure for table `note`
--

CREATE TABLE IF NOT EXISTS `note` (
  `ID` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `idPdf` int(10) unsigned NOT NULL,
  `idCompetence` varchar(10) NOT NULL COMMENT 'ex: cp2.2 ; autonomie ; ...',
  `Note` varchar(10) NOT NULL,
  `maxNote` varchar(10) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `idTp` (`idPdf`),
  KEY `idCompetence` (`idCompetence`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COMMENT='une ligne pour chaque note' AUTO_INCREMENT=106 ;

--
-- Dumping data for table `note`
--

INSERT INTO `note` (`ID`, `idPdf`, `idCompetence`, `Note`, `maxNote`) VALUES
(71, 27, 'C2.3', '7.5 ', ' 7.5\r'),
(72, 27, 'C3.2', '11.5 ', ' 11.5\r'),
(73, 28, 'C2.3', '17 ', ' 17\r'),
(74, 29, 'C1.1', '4 ', '9 '),
(75, 29, 'C2.1', '20 ', '20 '),
(76, 29, 'C3.1', '2\r\n', '2\r'),
(77, 30, 'C1.1', '6 ', '4 '),
(78, 30, 'C1.2', '17 ', '20 '),
(79, 30, 'C1.3', '2\r\n', '3\r'),
(80, 31, 'C2.1', '10 ', '4 '),
(81, 31, 'C2.3', '14 ', '20 '),
(82, 31, 'C3.2', '1\r\n', '2\r'),
(83, 32, 'C2.3', '11 ', ' 11\r'),
(84, 32, 'C3.2', '7', ' 7\r'),
(85, 33, 'C1.1', '4 ', '9 '),
(86, 33, 'C2.1', '10 ', '20 '),
(87, 33, 'C3.1', '1\r\n', '2\r'),
(88, 34, 'C1.1', '6 ', '4 '),
(89, 34, 'C1.2', '10 ', '20 '),
(90, 34, 'C1.3', '1\r\n', '3\r'),
(91, 35, 'C2.1', '10 ', '4 '),
(92, 35, 'C2.3', '15 ', '20 '),
(93, 35, 'C3.2', '2\r\n', '2\r'),
(94, 36, 'C1.5', '11 ', ' 11\r'),
(95, 36, 'C1.7', '6.5 ', ' 6.5\r'),
(96, 36, 'C2.3', '1.5 ', ' 1.5\r'),
(97, 37, 'C1.1', '4 ', '9 '),
(98, 37, 'C2.1', '22 ', '20 '),
(99, 37, 'C3.1', '2\r\n', '2\r'),
(100, 38, 'C1.1', '6 ', '4 '),
(101, 38, 'C1.2', '20 ', '20 '),
(102, 38, 'C1.3', '3\r\n', '3\r'),
(103, 39, 'C2.1', '10 ', '4 '),
(104, 39, 'C2.3', '20 ', '20 '),
(105, 39, 'C3.2', '2\r\n', '2\r');

-- --------------------------------------------------------

--
-- Table structure for table `tp`
--

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
  KEY `idcorrecteur` (`idcorrecteur`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COMMENT='une ligne pour chaque PDF de lu' AUTO_INCREMENT=40 ;

--
-- Dumping data for table `tp`
--

INSERT INTO `tp` (`idPdf`, `idTp`, `idEleve`, `idcorrecteur`, `date`, `hashTp`) VALUES
(27, 'Hydraulique compacticc montage differentiel astriane corrige', 7, 2, '2016-04-30 09:11:27', 'c0981af420f5aab8f3b98d9643b7c210'),
(28, 'Hydraulique extrudeuse centre ouvert astriane corrige', 7, 2, '2016-04-30 09:11:28', '8df3d7141cbee47c71f62d6eed7ca8ba'),
(29, 'Tp maintenance 1', 7, 2, '2016-04-30 09:11:28', 'fafa60e93e813e5441363069ba42ce8a'),
(30, 'Tp maintenance 2', 7, 2, '2016-04-30 09:11:28', '562d161377afcb4a2ad919646978ca3d'),
(31, 'Tp maintenance 3', 7, 2, '2016-04-30 09:11:28', 'bab3fae414dc9a0e7e5df5eafd929531'),
(32, 'Hydraulique extrudeuse Le pressostat astriane corrige', 8, 2, '2016-04-30 09:11:32', '209e0daa84a9742d4017364c0e6476da'),
(33, 'Tp maintenance 1', 8, 2, '2016-04-30 09:11:32', '65b5f5c1026a84a2d9803e81b21ad323'),
(34, 'Tp maintenance 2', 8, 2, '2016-04-30 09:11:32', '31067c260081f02aa88fcef34c2c772d'),
(35, 'Tp maintenance 3', 8, 2, '2016-04-30 09:11:32', '13e9728660dc385092c6f0b10309d13a'),
(36, 'Changement de technologie extrudicc astriane corrige', 9, 2, '2016-04-30 09:11:33', '7d24e39b2896fb33c25ab23ead02777f'),
(37, 'Tp maintenance 1', 9, 2, '2016-04-30 09:11:33', '60bee2555f068c4a1969be121846d17a'),
(38, 'Tp maintenance 2', 9, 2, '2016-04-30 09:11:33', 'bec8f578788dcc86d753e216e825bb7e'),
(39, 'Tp maintenance 3', 9, 2, '2016-04-30 09:11:33', '3e73c98fc8f8b7c202e6c290c2014c5a');

-- --------------------------------------------------------

--
-- Table structure for table `user`
--

CREATE TABLE IF NOT EXISTS `user` (
  `idUser` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Login` varchar(32) NOT NULL,
  `Password` varchar(32) NOT NULL,
  `Admin` tinyint(1) DEFAULT '0' COMMENT 'si 1 => admin',
  PRIMARY KEY (`idUser`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COMMENT='une ligne pour chaque utilisateur (prof)' AUTO_INCREMENT=3 ;

--
-- Dumping data for table `user`
--

INSERT INTO `user` (`idUser`, `Login`, `Password`, `Admin`) VALUES
(2, 'a', 'a', 1);

--
-- Constraints for dumped tables
--

--
-- Constraints for table `eleve`
--
ALTER TABLE `eleve`
  ADD CONSTRAINT `fk_classe_eleve` FOREIGN KEY (`idClasse`) REFERENCES `classe` (`idClasse`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `note`
--
ALTER TABLE `note`
  ADD CONSTRAINT `fk_note_tp` FOREIGN KEY (`idPdf`) REFERENCES `tp` (`idPdf`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `tp`
--
ALTER TABLE `tp`
  ADD CONSTRAINT `fk_tp_eleve` FOREIGN KEY (`idEleve`) REFERENCES `eleve` (`idEleve`) ON DELETE CASCADE ON UPDATE CASCADE;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
