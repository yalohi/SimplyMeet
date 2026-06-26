#!/bin/bash
set -euo pipefail

readonly SCRIPT_DIR="$(cd "$(dirname "${0}")"; pwd -P)"
readonly SM_MAIN_DIR="${SCRIPT_DIR}/.."

readonly SM_BUILD_CONFIG="${1:-"release"}"
readonly SM_BUILD_ARCH="${2:-}"
readonly SM_BUILD_NAME="${3:-}"

readonly SM_BUILD_DIR="Build"
readonly SM_WASM_PROJECT="${SM_MAIN_DIR}/SimplyMeetWasm/SimplyMeetWasm.csproj"
readonly SM_API_PROJECT="${SM_MAIN_DIR}/SimplyMeetApi/SimplyMeetApi.csproj"
readonly SM_WASM_BUILD_DIR="${SM_BUILD_DIR}/SimplyMeetWasm"
readonly SM_API_BUILD_DIR="${SM_BUILD_DIR}/SimplyMeetApi"

[ -z "${SM_BUILD_ARCH}" ] || readonly SM_RID_OPTION="-r ${SM_BUILD_ARCH}"

cd "${SM_MAIN_DIR}"

if [ "${SM_BUILD_CONFIG^^}" = "DEBUG" ]; then
	[ -f "${SM_WASM_PROJECT}" ] &&  [ -z "${SM_BUILD_NAME}" -o "${SM_BUILD_NAME}" = "wasm" ] && \
		dotnet publish "${SM_WASM_PROJECT}" \
			-c "${SM_BUILD_CONFIG}" \
			-o "${SM_WASM_BUILD_DIR}" \
			${SM_RID_OPTION:-} \
			-p:SelfContained=True \
			-p:PublishTrimmed=False \
			-p:BlazorEnableCompression=False

	[ -f "${SM_API_PROJECT}" ] && [ -z "${SM_BUILD_NAME}" -o "${SM_BUILD_NAME}" = "api" ] && \
		dotnet publish "${SM_API_PROJECT}" \
			-c "${SM_BUILD_CONFIG}" \
			-o "${SM_API_BUILD_DIR}" \
			${SM_RID_OPTION:-} \
			-p:SelfContained=True \
			-p:PublishReadyToRun=False \
			-p:PublishSingleFile=True \
			-p:PublishTrimmed=False \
			-p:IncludeNativeLibrariesForSelfExtract=True
elif [ "${SM_BUILD_CONFIG^^}" = "RELEASE" ]; then
	[ -f "${SM_WASM_PROJECT}" ] && [ -z "${SM_BUILD_NAME}" -o "${SM_BUILD_NAME}" = "wasm" ] && \
		dotnet publish "${SM_WASM_PROJECT}" \
			-c "${SM_BUILD_CONFIG}" \
			-o "${SM_WASM_BUILD_DIR}" \
			${SM_RID_OPTION:-} \
			-p:SelfContained=True \
			-p:PublishTrimmed=True \
			-p:BlazorEnableCompression=True

	[ -f "${SM_API_PROJECT}" ] && [ -z "${SM_BUILD_NAME}" -o "${SM_BUILD_NAME}" = "api" ] && \
		dotnet publish "${SM_API_PROJECT}" \
			-c "${SM_BUILD_CONFIG}" \
			-o "${SM_API_BUILD_DIR}" \
			${SM_RID_OPTION:-} \
			-p:SelfContained=True \
			-p:PublishReadyToRun=False \
			-p:PublishSingleFile=True \
			-p:PublishTrimmed=True \
			-p:IncludeNativeLibrariesForSelfExtract=True

	rm -vf \
		"${SM_WASM_BUILD_DIR}/"*".pdb" \
		"${SM_API_BUILD_DIR}/"*".pdb"
fi
