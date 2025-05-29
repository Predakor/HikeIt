import { Provider } from "@/components/ui/provider";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { BrowserRouter, Route, Routes } from "react-router";
import Layout from "./Layout/Layout";
import { routes } from "./data/routes";
import "./index.css";

const queryClient = new QueryClient();

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <BrowserRouter>
      <Provider>
        <QueryClientProvider client={queryClient}>
          <Layout>
            <Routes>
              {routes.map(({ path, Page }) => (
                <Route path={path} element={<Page />} key={path} />
              ))}
            </Routes>
          </Layout>
        </QueryClientProvider>
      </Provider>
    </BrowserRouter>
  </StrictMode>
);
