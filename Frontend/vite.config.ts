import plugin from "@vitejs/plugin-react";
import { defineConfig } from "vite";
import { analyzer } from "vite-bundle-analyzer";
import tsconfigPaths from "vite-tsconfig-paths";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [plugin(), tsconfigPaths(), analyzer()],
  server: {
    port: 54840,
  },
});
