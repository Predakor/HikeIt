import { Provider } from "@/components/ui/provider";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { BrowserRouter } from "react-router";
import Layout from "./Layout/Layout";
import RenderRoutes from "./components/RenderRoutes/RenderRoutes";
import "./index.css";

const queryClient = new QueryClient();

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <BrowserRouter>
      <Provider>
        <QueryClientProvider client={queryClient}>
          <Layout>
            <RenderRoutes />
          </Layout>
        </QueryClientProvider>
      </Provider>
    </BrowserRouter>
  </StrictMode>
);
