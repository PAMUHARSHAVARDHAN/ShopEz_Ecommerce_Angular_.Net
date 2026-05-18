# =========================
# Build Stage
# =========================

FROM node:22-alpine AS build

WORKDIR /app

# Copy package files
COPY package*.json ./

# Clean install dependencies
RUN npm ci

# Copy source code
COPY . .

# Build Angular production app
RUN npm run build -- --configuration production --base-href /


# =========================
# Runtime Stage
# =========================

FROM nginx:alpine

# Copy Angular build output
COPY --from=build /app/dist/my-app/browser /usr/share/nginx/html


# Expose nginx port
EXPOSE 80

# Start nginx
CMD ["nginx", "-g", "daemon off;"]