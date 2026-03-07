import { Provider } from "@/components/ui/provider";
import { QueryClient } from "@tanstack/react-query";
import { StrictMode } from "react";
import { BrowserRouter } from "react-router";
import Layout from "./Layout/Layout";
import RenderRoutes from "./components/Utils/RenderRoutes/RenderRoutes";
import { createAsyncStoragePersister } from "@tanstack/query-async-storage-persister";
import { PersistQueryClientProvider } from "@tanstack/react-query-persist-client";

const baseName = (import.meta.env.VITE_BASE_PATH as string).replace(/\/$/, "");

export default function App() {
  const queryClient = new QueryClient();

  const asyncStoragePersister = createAsyncStoragePersister({
    storage: window.localStorage,
  });

  return (
    <StrictMode>
      <BrowserRouter basename={baseName}>
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
}
