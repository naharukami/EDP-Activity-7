-- MySQL dump 10.13  Distrib 8.0.46, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: act2
-- ------------------------------------------------------
-- Server version	5.5.5-10.4.32-MariaDB

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `animes`
--

DROP TABLE IF EXISTS `animes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `animes` (
  `AnimeID` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(255) NOT NULL,
  `ReleaseYear` int(11) DEFAULT NULL,
  `Rating` decimal(3,2) DEFAULT NULL,
  `StudioID` int(11) DEFAULT NULL,
  PRIMARY KEY (`AnimeID`),
  KEY `StudioID` (`StudioID`),
  CONSTRAINT `animes_ibfk_1` FOREIGN KEY (`StudioID`) REFERENCES `studios` (`StudioID`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `animes`
--

LOCK TABLES `animes` WRITE;
/*!40000 ALTER TABLE `animes` DISABLE KEYS */;
INSERT INTO `animes` VALUES (1,'Jujutsu Kaisen',2020,8.70,1),(2,'Demon Slayer',2019,8.60,2),(3,'Hunter x Hunter',2011,9.00,3),(4,'Attack on Titan',2013,9.10,4),(5,'My Hero Academia',2016,7.90,5),(6,'Sword Art Online',2012,7.20,6),(7,'Spy x Family',2022,8.50,7),(8,'Violet Evergarden',2018,8.90,8),(9,'One Piece',1999,8.70,9),(10,'Naruto',2002,8.30,10);
/*!40000 ALTER TABLE `animes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `studios`
--

DROP TABLE IF EXISTS `studios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `studios` (
  `StudioID` int(11) NOT NULL AUTO_INCREMENT,
  `StudioName` varchar(100) NOT NULL,
  `Location` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`StudioID`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `studios`
--

LOCK TABLES `studios` WRITE;
/*!40000 ALTER TABLE `studios` DISABLE KEYS */;
INSERT INTO `studios` VALUES (1,'MAPPA','Tokyo'),(2,'Ufotable','Tokyo'),(3,'Madhouse','Tokyo'),(4,'Wit Studio','Tokyo'),(5,'Bones','Tokyo'),(6,'A-1 Pictures','Tokyo'),(7,'CloverWorks','Tokyo'),(8,'Kyoto Animation','Kyoto'),(9,'Toei Animation','Tokyo'),(10,'Pierrot','Tokyo');
/*!40000 ALTER TABLE `studios` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `system_transactions`
--

DROP TABLE IF EXISTS `system_transactions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `system_transactions` (
  `transaction_id` int(11) NOT NULL AUTO_INCREMENT,
  `tutor_name` varchar(100) DEFAULT NULL,
  `student_name` varchar(100) DEFAULT NULL,
  `subject_area` varchar(50) DEFAULT NULL,
  `hours_rendered` int(11) DEFAULT NULL,
  `rating_score` decimal(3,2) DEFAULT NULL,
  `logged_by` varchar(50) DEFAULT NULL,
  `transaction_date` timestamp NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`transaction_id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `system_transactions`
--

LOCK TABLES `system_transactions` WRITE;
/*!40000 ALTER TABLE `system_transactions` DISABLE KEYS */;
INSERT INTO `system_transactions` VALUES (1,'Juan Dela Cruz','Maria Santos','Mathematics',2,4.80,'admin_noriel','2026-05-17 05:07:12'),(2,'Hannah Vicente','Pedro Penduko','Computer Literacy',3,4.50,'admin_noriel','2026-05-17 05:07:12'),(3,'Noriel Malate','Ana Rey','Information Systems',4,5.00,'hannah_admin','2026-05-17 05:07:12'),(4,'Mark Alcala','Julius Cezar','Reading Comprehension',2,4.20,'admin_noriel','2026-05-17 05:07:12'),(5,'Grace Imperial','Liza Maza','General Science',3,4.90,'hannah_admin','2026-05-17 05:07:12'),(6,'John Doe','Ryan Agoncillo','Basic English',1,4.00,'admin_noriel','2026-05-17 05:07:12'),(7,'Jane Smith','Sarah Geronimo','Mathematics',3,4.75,'hannah_admin','2026-05-17 05:07:12'),(8,'Christian Noel','Alden Richards','Computer Literacy',5,4.95,'admin_noriel','2026-05-17 05:07:12'),(9,'Patricia Diaz','Kathryn Bernardo','General Science',2,4.60,'hannah_admin','2026-05-17 05:07:12'),(10,'Anthony Ramos','Daniel Padilla','Reading Comprehension',4,4.85,'admin_noriel','2026-05-17 05:07:12');
/*!40000 ALTER TABLE `system_transactions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `UserID` int(11) NOT NULL AUTO_INCREMENT,
  `Username` varchar(50) DEFAULT NULL,
  `JoinDate` date DEFAULT NULL,
  `Password` varchar(255) DEFAULT 'password123',
  `Status` enum('Active','Inactive') DEFAULT 'Active',
  `role` varchar(20) DEFAULT 'User',
  PRIMARY KEY (`UserID`),
  UNIQUE KEY `Username` (`Username`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'Jessica','2023-01-01','Dannica2','Active','User'),(2,'GokuFan','2023-02-15','password123','Active','User'),(3,'ZoroLost','2023-03-10','password123','Inactive','User'),(4,'LuffyCaptain','2023-04-05','password123','Active','User'),(5,'MikasaLove','2023-05-20','password123','Active','User'),(6,'LightYagami','2023-06-12','password123','Inactive','User'),(7,'EdwardElric','2023-07-01','password123','Active','User'),(8,'SpikeMirror','2023-08-14','password123','Active','User'),(9,'SaitamaOne','2023-09-09','password123','Inactive','User'),(10,'KakashiSensei','2023-10-31','password123','Active','User'),(11,'Airo',NULL,'password123','Inactive','User'),(12,'AiroS',NULL,'1','Active','User'),(13,'AIROOOOO',NULL,'1','Active','User'),(14,'nori',NULL,'2','Active','Admin'),(15,'test_inactive',NULL,'password123','Inactive','User'),(16,'alejandro',NULL,'1','Active','User');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `view_topratedanime`
--

DROP TABLE IF EXISTS `view_topratedanime`;
/*!50001 DROP VIEW IF EXISTS `view_topratedanime`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `view_topratedanime` AS SELECT 
 1 AS `Title`,
 1 AS `Rating`,
 1 AS `ReleaseYear`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `view_useractivity`
--

DROP TABLE IF EXISTS `view_useractivity`;
/*!50001 DROP VIEW IF EXISTS `view_useractivity`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `view_useractivity` AS SELECT 
 1 AS `Username`,
 1 AS `AnimeName`,
 1 AS `Status`,
 1 AS `Score`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `watchlists`
--

DROP TABLE IF EXISTS `watchlists`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `watchlists` (
  `WatchID` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) DEFAULT NULL,
  `AnimeID` int(11) DEFAULT NULL,
  `Status` varchar(20) DEFAULT 'Watching',
  `Score` int(11) DEFAULT NULL,
  PRIMARY KEY (`WatchID`),
  KEY `watchlists_ibfk_1` (`UserID`),
  KEY `watchlists_ibfk_2` (`AnimeID`),
  CONSTRAINT `watchlists_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `users` (`UserID`),
  CONSTRAINT `watchlists_ibfk_2` FOREIGN KEY (`AnimeID`) REFERENCES `animes` (`AnimeID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `watchlists`
--

LOCK TABLES `watchlists` WRITE;
/*!40000 ALTER TABLE `watchlists` DISABLE KEYS */;
/*!40000 ALTER TABLE `watchlists` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Final view structure for view `view_topratedanime`
--

/*!50001 DROP VIEW IF EXISTS `view_topratedanime`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_topratedanime` AS select `animes`.`Title` AS `Title`,`animes`.`Rating` AS `Rating`,`animes`.`ReleaseYear` AS `ReleaseYear` from `animes` order by `animes`.`Rating` desc */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `view_useractivity`
--

/*!50001 DROP VIEW IF EXISTS `view_useractivity`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_useractivity` AS select `u`.`Username` AS `Username`,`a`.`Title` AS `AnimeName`,`w`.`Status` AS `Status`,`w`.`Score` AS `Score` from ((`users` `u` join `watchlists` `w` on(`u`.`UserID` = `w`.`UserID`)) join `animes` `a` on(`w`.`AnimeID` = `a`.`AnimeID`)) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-05-17 14:40:42
