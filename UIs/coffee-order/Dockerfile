# Stage 1: Build Angular app
FROM node:18 as build
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build -- --configuration production

# Stage 2: Serve with NGINX
FROM nginx:alpine
COPY --from=build /app/dist/coffee-order/browser /usr/share/nginx/html
EXPOSE 80
