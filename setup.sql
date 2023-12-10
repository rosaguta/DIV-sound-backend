-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema DIVSOUND
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema DIVSOUND
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `DIVSOUND` DEFAULT CHARACTER SET latin1 ;
USE `DIVSOUND` ;

-- -----------------------------------------------------
-- Table `DIVSOUND`.`user`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `DIVSOUND`.`user` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `firstname` VARCHAR(250) NULL DEFAULT NULL,
  `lastname` VARCHAR(250) NULL DEFAULT NULL,
  `mail` VARCHAR(250) NULL DEFAULT NULL,
  `passhash` VARCHAR(250) NULL DEFAULT NULL,
  `username` VARCHAR(250) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 7
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `DIVSOUND`.`audiofile`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `DIVSOUND`.`audiofile` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NULL DEFAULT NULL,
  `path` VARCHAR(250) NULL DEFAULT NULL,
  `duration` TIME NULL DEFAULT NULL,
  `uploaddate` DATETIME NULL DEFAULT NULL,
  `uploaderid` INT(11) NULL DEFAULT NULL,
  `url` VARCHAR(250) NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `uploaderid_idx` (`uploaderid` ASC) VISIBLE,
  CONSTRAINT `uploadid`
    FOREIGN KEY (`uploaderid`)
    REFERENCES `DIVSOUND`.`user` (`id`)
    ON DELETE CASCADE
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 179
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `DIVSOUND`.`board`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `DIVSOUND`.`board` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(250) NULL DEFAULT NULL,
  `userid` INT(11) NULL DEFAULT NULL,
  `sessionid` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `userid_idx` (`userid` ASC) VISIBLE,
  CONSTRAINT `userid`
    FOREIGN KEY (`userid`)
    REFERENCES `DIVSOUND`.`user` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 105
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `DIVSOUND`.`user_board_audio`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `DIVSOUND`.`user_board_audio` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `userid` INT(11) NULL DEFAULT NULL,
  `audioid` INT(11) NULL DEFAULT NULL,
  `boardid` INT(11) NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `user_idx` (`userid` ASC) VISIBLE,
  INDEX `board_idx` (`boardid` ASC) VISIBLE,
  INDEX `audio_idx` (`audioid` ASC) VISIBLE,
  CONSTRAINT `audio`
    FOREIGN KEY (`audioid`)
    REFERENCES `DIVSOUND`.`audiofile` (`id`)
    ON DELETE CASCADE
    ON UPDATE NO ACTION,
  CONSTRAINT `board`
    FOREIGN KEY (`boardid`)
    REFERENCES `DIVSOUND`.`board` (`id`)
    ON DELETE CASCADE
    ON UPDATE NO ACTION,
  CONSTRAINT `user`
    FOREIGN KEY (`userid`)
    REFERENCES `DIVSOUND`.`user` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 366
DEFAULT CHARACTER SET = latin1;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
