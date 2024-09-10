# Stage 1: Build the Angular app
FROM node:20.12.2 as build

# Set the working directory
WORKDIR /usr/src/app

# Copy the package.json and package-lock.json files
COPY package*.json ./

# Install Angular CLI and project dependencies
RUN npm install -g @angular/cli@17.3.9 --timeout=600000
RUN npm install --timeout=600000

# Copy the application code
COPY . .

# Build the Angular app
RUN ng build --configuration production

# Stage 2: Serve the app with Nginx
FROM nginx:alpine

# Copy the built app from Stage 1
COPY --from=build /usr/src/app/dist/easypos-client /usr/share/nginx/html

# Expose port 7105
EXPOSE 7105

# Command to start Nginx
CMD ["nginx", "-g", "daemon off;"]

# docker build -t easypos-client .
# docker run -d -p 7105:80 --name easypos-client easypos-client