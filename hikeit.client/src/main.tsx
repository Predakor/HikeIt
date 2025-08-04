import { Provider } from "@/components/ui/provider";
import { QueryClient } from "@tanstack/react-query";
import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { BrowserRouter } from "react-router";
import Layout from "./Layout/Layout";
import RenderRoutes from "./components/RenderRoutes/RenderRoutes";

import { createAsyncStoragePersister } from "@tanstack/query-async-storage-persister";
import { PersistQueryClientProvider } from "@tanstack/react-query-persist-client";
import "./index.css";

const queryClient = new QueryClient();

const asyncStoragePersister = createAsyncStoragePersister({
  storage: window.localStorage,
});

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <BrowserRouter>
      <Provider>
        <PersistQueryClientProvider
          client={queryClient}
          persistOptions={{ persister: asyncStoragePersister }}
        >
          <Layout>
            <RenderRoutes />
          </Layout>
        </PersistQueryClientProvider>
      </Provider>
    </BrowserRouter>
  </StrictMode>
);
