import { defineConfig, loadEnv } from "vite";
import plugin from "@vitejs/plugin-react";
import tsconfigPaths from "vite-tsconfig-paths";
import { analyzer } from "vite-bundle-analyzer";

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), "");
  return {
    base: env.VITE_BASE_PATH || "/",
    plugins: [plugin(), tsconfigPaths(), analyzer()],
    server: {
      port: 54840,
    },
  };
});
