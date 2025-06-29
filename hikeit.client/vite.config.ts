import { defineConfig } from "vite";
import plugin from "@vitejs/plugin-react";
import tsconfigPaths from "vite-tsconfig-paths";
import { analyzer } from "vite-bundle-analyzer";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [plugin(), tsconfigPaths(), analyzer()],
  server: {
    port: 54840,
  },
});
