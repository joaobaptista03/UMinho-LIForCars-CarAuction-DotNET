# Makefile for .NET Core project

# Project directory
PROJECT_DIR=LIForCars/

# Default target executed when no arguments are given to make.
default: run

# Target to run the project
run:
	dotnet run --project $(PROJECT_DIR)

# Target to clean the project
clean:
	rm -rf $(PROJECT_DIR)/bin
	rm -rf $(PROJECT_DIR)/obj
	rm -rf $(PROJECT_DIR)/logs

.PHONY: default run clean