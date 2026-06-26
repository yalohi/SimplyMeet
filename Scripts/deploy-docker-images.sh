#!/bin/bash
set -euo pipefail

readonly SCRIPT_DIR="$(cd "$(dirname "${0}")"; pwd -P)"
readonly SM_MAIN_DIR="${SCRIPT_DIR}/.."
readonly CONTAINER_REGISTRY="ghcr.io"
readonly CURRENT_LOGIN=$(docker login --get-login ${CONTAINER_REGISTRY} || true)
readonly DEPLOY_TAG=${1:-"latest"}

deploy_docker_image()
{
	local -r name="${1}"

	docker compose build ${name}-source-local
	docker tag localhost/simplymeet-${name}:${DEPLOY_TAG} ghcr.io/yalohi/simplymeet-${name}:${DEPLOY_TAG}
	docker push ghcr.io/yalohi/simplymeet-${name}:${DEPLOY_TAG}
}

cd "${SM_MAIN_DIR}/Docker"

[ -z "${CURRENT_LOGIN}" ] && docker login ${CONTAINER_REGISTRY} -u yalohi
deploy_docker_image "wasm"
deploy_docker_image "api"
