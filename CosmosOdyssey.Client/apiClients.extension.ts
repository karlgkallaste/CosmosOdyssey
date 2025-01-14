/// <reference types="vite/client" />
export class ClientBase {
    private _handleGlobalConflict = true;

    public disableGlobalConflictHandling() {
        this._handleGlobalConflict = false;
    }

    getBaseUrl(defaultUrl: string, baseUrl?: string) {
        const apiUrl = import.meta.env.VITE_API_BASE_URL;
        return apiUrl;
    }

    protected transformOptions(options: any) {
        const headers = new Headers(options.headers);

        options.headers = headers;
        return Promise.resolve(options);
    }

    protected transformResult(url: any, response: any, handleResponse: any): any {

        if (response.status == 500) {
            if (confirm("Something went wrong. Refresh the page?")) {
                location.reload();
                return;
            }
        }
        const errorStatusCodes = [403, 404, 429];

        return handleResponse(response);
    }

}
