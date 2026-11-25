import plugin from "@vitejs/plugin-react";
import { defineConfig } from "vite";
import { analyzer } from "vite-bundle-analyzer";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [plugin(), analyzer()],
  resolve: {
    tsconfigPaths: true,
  },
  server: {
    port: 54840,
  },

  optimizeDeps: {
    exclude: ["maplibre-gl"],
  },

  build: {
    target: "es2022",
  },
});
