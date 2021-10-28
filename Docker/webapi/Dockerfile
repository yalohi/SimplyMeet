FROM mcr.microsoft.com/dotnet/runtime-deps:5.0-alpine
RUN apk add --update sqlite
COPY app /app
WORKDIR /app
ENTRYPOINT "./SimplyMeetApi"
