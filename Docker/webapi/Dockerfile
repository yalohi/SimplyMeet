FROM mcr.microsoft.com/dotnet/runtime-deps:7.0-alpine
RUN apk add --update sqlite
COPY app /app
WORKDIR /app
ENTRYPOINT "./SimplyMeetApi"
