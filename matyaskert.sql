CREATE DATABASE `matyaskert`;

CREATE TABLE `matyaskert`.`User` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `username` VARCHAR(256) NOT NULL,
  `password` VARCHAR(128) NOT NULL,
  `realName` VARCHAR(256) NOT NULL,
  `email` VARCHAR(256) NOT NULL,
  `isActive` TINYINT NULL DEFAULT 0,
  `isAdmin` TINYINT NULL DEFAULT 0,
  PRIMARY KEY (`id`));

CREATE TABLE `matyaskert`.`Contest` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(256) NOT NULL,
  `startDate` DATETIME NOT NULL,
  `endDate` DATETIME NOT NULL,
  `isOpened` TINYINT NULL DEFAULT 0,
  PRIMARY KEY (`id`));

CREATE TABLE `matyaskert`.`ScoringRules` (
  `description` VARCHAR(256) NOT NULL,
  `points` INT NOT NULL,
  `contestId` INT NOT NULL,

  FOREIGN KEY (`contestId`)
    REFERENCES `matyaskert`.`Contest` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);

CREATE TABLE `matyaskert`.`Standings` (
  `points` INT NULL DEFAULT 0,
  `contestId` INT NOT NULL,
  `userId` INT NOT NULL,
  
  FOREIGN KEY (`contestId`)
    REFERENCES `matyaskert`.`Contest` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  FOREIGN KEY (`userId`)
    REFERENCES `matyaskert`.`User` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);

CREATE TABLE `matyaskert`.`Match` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `homeName` VARCHAR(256) NOT NULL,
  `guestName` VARCHAR(256) NOT NULL,
  `date` DATETIME NOT NULL,
  `homeGoals` INT NULL,
  `guestGoals` INT NULL,
  `isAvailable` TINYINT NULL DEFAULT 0,
  PRIMARY KEY (`id`));

CREATE TABLE `matyaskert`.`InContest` (
  `contestId` INT NOT NULL,
  `matchId` INT NOT NULL,
  
  FOREIGN KEY (`contestId`)
    REFERENCES `matyaskert`.`Contest` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  FOREIGN KEY (`matchId`)
    REFERENCES `matyaskert`.`Match` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);

CREATE TABLE `matyaskert`.`Bet` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `contestId` INT NOT NULL,
  `userId` INT NOT NULL,
  `matchId` INT NOT NULL,
  `homeGoals` INT NOT NULL,
  `guestGoals` INT NOT NULL,
  `isWon` TINYINT NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  
  FOREIGN KEY (`contestId`)
    REFERENCES `matyaskert`.`Contest` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  FOREIGN KEY (`userId`)
    REFERENCES `matyaskert`.`User` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  FOREIGN KEY (`matchId`)
    REFERENCES `matyaskert`.`Match` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);




