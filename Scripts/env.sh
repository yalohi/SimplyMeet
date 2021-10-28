#!/bin/bash

MY_DOCKER_PATH=Docker/
MY_BUILD_PATH=Build/
MY_BACKUPS_PATH=Backups/
MY_BACKUP_DAYS=7
MY_BACKUP_NAME=$(date +"%Y-%m-%d-%H-%M-%S").tar
MY_TEMP_BACKUP_PATH=/backup
MY_DATA_PATH=/app/App_Data
MY_CONTAINER_NAME_WEBAPI=simplymeet-webapi-1
MY_VOLUME_NAME_WASM_APP=simplymeet_wasm-app
MY_VOLUME_NAME_WEBAPI_APP=simplymeet_webapi-app