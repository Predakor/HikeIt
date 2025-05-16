import { Provider } from "@/components/ui/provider";
import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { BrowserRouter, Route, Routes } from "react-router";
import "./index.css";
import Layout from "./Layout/Layout";
import { routes } from "./data/routes";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <BrowserRouter>
      <Provider>
        <Layout>
          <Routes>
            {routes.map(({ path, Page }) => (
              <Route path={path} element={<Page />} key={path} />
            ))}
          </Routes>
        </Layout>
      </Provider>
    </BrowserRouter>
  </StrictMode>
);
