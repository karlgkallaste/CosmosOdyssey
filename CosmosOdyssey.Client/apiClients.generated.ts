//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v14.2.0.0 (NJsonSchema v11.1.0.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

/* tslint:disable */
/* eslint-disable */
// ReSharper disable InconsistentNaming

export module api {
export class ClientBase {
    private _handleGlobalConflict = true;

    public disableGlobalConflictHandling() {
        this._handleGlobalConflict = false;
    }

    getBaseUrl(defaultUrl: string, baseUrl?: string) {
        return "https://localhost:7299";
    }

    protected transformOptions(options: any) {
        const headers = new Headers(options.headers);

        options.headers = headers;
        return Promise.resolve(options);
    }

    protected transformResult(url: any, response: any, handleResponse: any): any {

        if (response.status == 401) {
            return;
        }
        if (response.status == 409) {
            if (!this._handleGlobalConflict) {
                return Promise.reject(response);
            }
            window.alert("Ressurss on vahepeal muutunud, palun värskendage lehte.");
            return Promise.reject(response);
        }
        if (response.status == 500) {
            if (confirm("Midagi läks valesti. Kas värskendan lehte?")) {
                location.reload();
                return;
            }
        }
        const errorStatusCodes = [403, 404, 429];

        return handleResponse(response);
    }

}

export class ReservationClient extends ClientBase {
    private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> }) {
        super();
        this.http = http ? http : window as any;
        this.baseUrl = this.getBaseUrl("", baseUrl);
    }

    create(request: CreateReservationRequest): Promise<string> {
        let url_ = this.baseUrl + "/Reservation/create";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(request);

        let options_: RequestInit = {
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.transformResult(url_, _response, (_response: Response) => this.processCreate(_response));
        });
    }

    protected processCreate(response: Response): Promise<string> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        let _mappings: { source: any, target: any }[] = [];
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : jsonParse(_responseText, this.jsonParseReviver);
                result200 = resultData200 !== undefined ? resultData200 : <any>null;
    
            return result200;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : jsonParse(_responseText, this.jsonParseReviver);
            result400 = BadRequest.fromJS(resultData400, _mappings);
            return throwException("A server side error occurred.", status, _responseText, _headers, result400);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<string>(null as any);
    }
}

export class LegClient extends ClientBase {
    private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> }) {
        super();
        this.http = http ? http : window as any;
        this.baseUrl = this.getBaseUrl("", baseUrl);
    }

    listFilters(): Promise<LegListFilterOptionsModel> {
        let url_ = this.baseUrl + "/Leg/list-filters";
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.transformResult(url_, _response, (_response: Response) => this.processListFilters(_response));
        });
    }

    protected processListFilters(response: Response): Promise<LegListFilterOptionsModel> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        let _mappings: { source: any, target: any }[] = [];
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : jsonParse(_responseText, this.jsonParseReviver);
            result200 = LegListFilterOptionsModel.fromJS(resultData200, _mappings);
            return result200;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : jsonParse(_responseText, this.jsonParseReviver);
            result400 = BadRequest.fromJS(resultData400, _mappings);
            return throwException("A server side error occurred.", status, _responseText, _headers, result400);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<LegListFilterOptionsModel>(null as any);
    }

    legs(from: string | undefined, to: string | undefined, start: Date | undefined, end: Date | undefined): Promise<RouteListItemModel[]> {
        let url_ = this.baseUrl + "/Leg/list?";
        if (from === null)
            throw new Error("The parameter 'from' cannot be null.");
        else if (from !== undefined)
            url_ += "From=" + encodeURIComponent("" + from) + "&";
        if (to === null)
            throw new Error("The parameter 'to' cannot be null.");
        else if (to !== undefined)
            url_ += "To=" + encodeURIComponent("" + to) + "&";
        if (start === null)
            throw new Error("The parameter 'start' cannot be null.");
        else if (start !== undefined)
            url_ += "Start=" + encodeURIComponent(start ? "" + start.toISOString() : "") + "&";
        if (end === null)
            throw new Error("The parameter 'end' cannot be null.");
        else if (end !== undefined)
            url_ += "End=" + encodeURIComponent(end ? "" + end.toISOString() : "") + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.transformResult(url_, _response, (_response: Response) => this.processLegs(_response));
        });
    }

    protected processLegs(response: Response): Promise<RouteListItemModel[]> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        let _mappings: { source: any, target: any }[] = [];
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : jsonParse(_responseText, this.jsonParseReviver);
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (let item of resultData200)
                    result200!.push(RouteListItemModel.fromJS(item, _mappings));
            }
            else {
                result200 = <any>null;
            }
            return result200;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : jsonParse(_responseText, this.jsonParseReviver);
            result400 = BadRequest.fromJS(resultData400, _mappings);
            return throwException("A server side error occurred.", status, _responseText, _headers, result400);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<RouteListItemModel[]>(null as any);
    }
}

export class BadRequest implements IBadRequest {
    statusCode?: number;

    constructor(data?: IBadRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any, _mappings?: any) {
        if (_data) {
            this.statusCode = _data["statusCode"] !== undefined ? _data["statusCode"] : <any>null;
        }
    }

    static fromJS(data: any, _mappings?: any): BadRequest | null {
        data = typeof data === 'object' ? data : {};
        return createInstance<BadRequest>(data, _mappings, BadRequest);
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["statusCode"] = this.statusCode !== undefined ? this.statusCode : <any>null;
        return data;
    }

    clone(): BadRequest {
        const json = this.toJSON();
        let result = new BadRequest();
        result.init(json);
        return result;
    }
}

export interface IBadRequest {
    statusCode?: number;
}

export class CreateReservationRequest implements ICreateReservationRequest {
    priceListId?: string;
    name?: PersonNameModel;
    routes?: ReservationRouteModel[];

    constructor(data?: ICreateReservationRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any, _mappings?: any) {
        if (_data) {
            this.priceListId = _data["priceListId"] !== undefined ? _data["priceListId"] : <any>null;
            this.name = _data["name"] ? PersonNameModel.fromJS(_data["name"], _mappings) : <any>null;
            if (Array.isArray(_data["routes"])) {
                this.routes = [] as any;
                for (let item of _data["routes"])
                    this.routes!.push(ReservationRouteModel.fromJS(item, _mappings));
            }
            else {
                this.routes = <any>null;
            }
        }
    }

    static fromJS(data: any, _mappings?: any): CreateReservationRequest | null {
        data = typeof data === 'object' ? data : {};
        return createInstance<CreateReservationRequest>(data, _mappings, CreateReservationRequest);
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["priceListId"] = this.priceListId !== undefined ? this.priceListId : <any>null;
        data["name"] = this.name ? this.name.toJSON() : <any>null;
        if (Array.isArray(this.routes)) {
            data["routes"] = [];
            for (let item of this.routes)
                data["routes"].push(item.toJSON());
        }
        return data;
    }

    clone(): CreateReservationRequest {
        const json = this.toJSON();
        let result = new CreateReservationRequest();
        result.init(json);
        return result;
    }
}

export interface ICreateReservationRequest {
    priceListId?: string;
    name?: PersonNameModel;
    routes?: ReservationRouteModel[];
}

export class PersonNameModel implements IPersonNameModel {
    firstName?: string;
    lastName?: string;

    constructor(data?: IPersonNameModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any, _mappings?: any) {
        if (_data) {
            this.firstName = _data["firstName"] !== undefined ? _data["firstName"] : <any>null;
            this.lastName = _data["lastName"] !== undefined ? _data["lastName"] : <any>null;
        }
    }

    static fromJS(data: any, _mappings?: any): PersonNameModel | null {
        data = typeof data === 'object' ? data : {};
        return createInstance<PersonNameModel>(data, _mappings, PersonNameModel);
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["firstName"] = this.firstName !== undefined ? this.firstName : <any>null;
        data["lastName"] = this.lastName !== undefined ? this.lastName : <any>null;
        return data;
    }

    clone(): PersonNameModel {
        const json = this.toJSON();
        let result = new PersonNameModel();
        result.init(json);
        return result;
    }
}

export interface IPersonNameModel {
    firstName?: string;
    lastName?: string;
}

export class ReservationRouteModel implements IReservationRouteModel {
    companyId?: string;
    legId?: string;

    constructor(data?: IReservationRouteModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any, _mappings?: any) {
        if (_data) {
            this.companyId = _data["companyId"] !== undefined ? _data["companyId"] : <any>null;
            this.legId = _data["legId"] !== undefined ? _data["legId"] : <any>null;
        }
    }

    static fromJS(data: any, _mappings?: any): ReservationRouteModel | null {
        data = typeof data === 'object' ? data : {};
        return createInstance<ReservationRouteModel>(data, _mappings, ReservationRouteModel);
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["companyId"] = this.companyId !== undefined ? this.companyId : <any>null;
        data["legId"] = this.legId !== undefined ? this.legId : <any>null;
        return data;
    }

    clone(): ReservationRouteModel {
        const json = this.toJSON();
        let result = new ReservationRouteModel();
        result.init(json);
        return result;
    }
}

export interface IReservationRouteModel {
    companyId?: string;
    legId?: string;
}

export class LegListFilterOptionsModel implements ILegListFilterOptionsModel {
    locations?: LocationModel[];
    companies?: CompanyModel[];

    constructor(data?: ILegListFilterOptionsModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any, _mappings?: any) {
        if (_data) {
            if (Array.isArray(_data["locations"])) {
                this.locations = [] as any;
                for (let item of _data["locations"])
                    this.locations!.push(LocationModel.fromJS(item, _mappings));
            }
            else {
                this.locations = <any>null;
            }
            if (Array.isArray(_data["companies"])) {
                this.companies = [] as any;
                for (let item of _data["companies"])
                    this.companies!.push(CompanyModel.fromJS(item, _mappings));
            }
            else {
                this.companies = <any>null;
            }
        }
    }

    static fromJS(data: any, _mappings?: any): LegListFilterOptionsModel | null {
        data = typeof data === 'object' ? data : {};
        return createInstance<LegListFilterOptionsModel>(data, _mappings, LegListFilterOptionsModel);
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        if (Array.isArray(this.locations)) {
            data["locations"] = [];
            for (let item of this.locations)
                data["locations"].push(item.toJSON());
        }
        if (Array.isArray(this.companies)) {
            data["companies"] = [];
            for (let item of this.companies)
                data["companies"].push(item.toJSON());
        }
        return data;
    }

    clone(): LegListFilterOptionsModel {
        const json = this.toJSON();
        let result = new LegListFilterOptionsModel();
        result.init(json);
        return result;
    }
}

export interface ILegListFilterOptionsModel {
    locations?: LocationModel[];
    companies?: CompanyModel[];
}

export class LocationModel implements ILocationModel {
    id?: string;
    name?: string;

    constructor(data?: ILocationModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any, _mappings?: any) {
        if (_data) {
            this.id = _data["id"] !== undefined ? _data["id"] : <any>null;
            this.name = _data["name"] !== undefined ? _data["name"] : <any>null;
        }
    }

    static fromJS(data: any, _mappings?: any): LocationModel | null {
        data = typeof data === 'object' ? data : {};
        return createInstance<LocationModel>(data, _mappings, LocationModel);
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id !== undefined ? this.id : <any>null;
        data["name"] = this.name !== undefined ? this.name : <any>null;
        return data;
    }

    clone(): LocationModel {
        const json = this.toJSON();
        let result = new LocationModel();
        result.init(json);
        return result;
    }
}

export interface ILocationModel {
    id?: string;
    name?: string;
}

export class CompanyModel implements ICompanyModel {
    id?: string;
    name?: string;

    constructor(data?: ICompanyModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any, _mappings?: any) {
        if (_data) {
            this.id = _data["id"] !== undefined ? _data["id"] : <any>null;
            this.name = _data["name"] !== undefined ? _data["name"] : <any>null;
        }
    }

    static fromJS(data: any, _mappings?: any): CompanyModel | null {
        data = typeof data === 'object' ? data : {};
        return createInstance<CompanyModel>(data, _mappings, CompanyModel);
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id !== undefined ? this.id : <any>null;
        data["name"] = this.name !== undefined ? this.name : <any>null;
        return data;
    }

    clone(): CompanyModel {
        const json = this.toJSON();
        let result = new CompanyModel();
        result.init(json);
        return result;
    }
}

export interface ICompanyModel {
    id?: string;
    name?: string;
}

export class RouteListItemModel implements IRouteListItemModel {
    priceListId?: string;
    routes?: RouteInfoModel[];

    constructor(data?: IRouteListItemModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any, _mappings?: any) {
        if (_data) {
            this.priceListId = _data["priceListId"] !== undefined ? _data["priceListId"] : <any>null;
            if (Array.isArray(_data["routes"])) {
                this.routes = [] as any;
                for (let item of _data["routes"])
                    this.routes!.push(RouteInfoModel.fromJS(item, _mappings));
            }
            else {
                this.routes = <any>null;
            }
        }
    }

    static fromJS(data: any, _mappings?: any): RouteListItemModel | null {
        data = typeof data === 'object' ? data : {};
        return createInstance<RouteListItemModel>(data, _mappings, RouteListItemModel);
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["priceListId"] = this.priceListId !== undefined ? this.priceListId : <any>null;
        if (Array.isArray(this.routes)) {
            data["routes"] = [];
            for (let item of this.routes)
                data["routes"].push(item.toJSON());
        }
        return data;
    }

    clone(): RouteListItemModel {
        const json = this.toJSON();
        let result = new RouteListItemModel();
        result.init(json);
        return result;
    }
}

export interface IRouteListItemModel {
    priceListId?: string;
    routes?: RouteInfoModel[];
}

export class RouteInfoModel implements IRouteInfoModel {
    id?: string;
    from?: LegInfoModel;
    to?: LegInfoModel;
    providers?: ProviderInfoModel[];

    constructor(data?: IRouteInfoModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any, _mappings?: any) {
        if (_data) {
            this.id = _data["id"] !== undefined ? _data["id"] : <any>null;
            this.from = _data["from"] ? LegInfoModel.fromJS(_data["from"], _mappings) : <any>null;
            this.to = _data["to"] ? LegInfoModel.fromJS(_data["to"], _mappings) : <any>null;
            if (Array.isArray(_data["providers"])) {
                this.providers = [] as any;
                for (let item of _data["providers"])
                    this.providers!.push(ProviderInfoModel.fromJS(item, _mappings));
            }
            else {
                this.providers = <any>null;
            }
        }
    }

    static fromJS(data: any, _mappings?: any): RouteInfoModel | null {
        data = typeof data === 'object' ? data : {};
        return createInstance<RouteInfoModel>(data, _mappings, RouteInfoModel);
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id !== undefined ? this.id : <any>null;
        data["from"] = this.from ? this.from.toJSON() : <any>null;
        data["to"] = this.to ? this.to.toJSON() : <any>null;
        if (Array.isArray(this.providers)) {
            data["providers"] = [];
            for (let item of this.providers)
                data["providers"].push(item.toJSON());
        }
        return data;
    }

    clone(): RouteInfoModel {
        const json = this.toJSON();
        let result = new RouteInfoModel();
        result.init(json);
        return result;
    }
}

export interface IRouteInfoModel {
    id?: string;
    from?: LegInfoModel;
    to?: LegInfoModel;
    providers?: ProviderInfoModel[];
}

export class LegInfoModel implements ILegInfoModel {
    id?: string;
    name?: string;

    constructor(data?: ILegInfoModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any, _mappings?: any) {
        if (_data) {
            this.id = _data["id"] !== undefined ? _data["id"] : <any>null;
            this.name = _data["name"] !== undefined ? _data["name"] : <any>null;
        }
    }

    static fromJS(data: any, _mappings?: any): LegInfoModel | null {
        data = typeof data === 'object' ? data : {};
        return createInstance<LegInfoModel>(data, _mappings, LegInfoModel);
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id !== undefined ? this.id : <any>null;
        data["name"] = this.name !== undefined ? this.name : <any>null;
        return data;
    }

    clone(): LegInfoModel {
        const json = this.toJSON();
        let result = new LegInfoModel();
        result.init(json);
        return result;
    }
}

export interface ILegInfoModel {
    id?: string;
    name?: string;
}

export class ProviderInfoModel implements IProviderInfoModel {
    id?: string;
    company?: CompanyInfoModel;
    price?: number;
    flightStart?: Date;
    flightEnd?: Date;

    constructor(data?: IProviderInfoModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any, _mappings?: any) {
        if (_data) {
            this.id = _data["id"] !== undefined ? _data["id"] : <any>null;
            this.company = _data["company"] ? CompanyInfoModel.fromJS(_data["company"], _mappings) : <any>null;
            this.price = _data["price"] !== undefined ? _data["price"] : <any>null;
            this.flightStart = _data["flightStart"] ? new Date(_data["flightStart"].toString()) : <any>null;
            this.flightEnd = _data["flightEnd"] ? new Date(_data["flightEnd"].toString()) : <any>null;
        }
    }

    static fromJS(data: any, _mappings?: any): ProviderInfoModel | null {
        data = typeof data === 'object' ? data : {};
        return createInstance<ProviderInfoModel>(data, _mappings, ProviderInfoModel);
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id !== undefined ? this.id : <any>null;
        data["company"] = this.company ? this.company.toJSON() : <any>null;
        data["price"] = this.price !== undefined ? this.price : <any>null;
        data["flightStart"] = this.flightStart ? this.flightStart.toISOString() : <any>null;
        data["flightEnd"] = this.flightEnd ? this.flightEnd.toISOString() : <any>null;
        return data;
    }

    clone(): ProviderInfoModel {
        const json = this.toJSON();
        let result = new ProviderInfoModel();
        result.init(json);
        return result;
    }
}

export interface IProviderInfoModel {
    id?: string;
    company?: CompanyInfoModel;
    price?: number;
    flightStart?: Date;
    flightEnd?: Date;
}

export class CompanyInfoModel implements ICompanyInfoModel {
    id?: string;
    name?: string;

    constructor(data?: ICompanyInfoModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any, _mappings?: any) {
        if (_data) {
            this.id = _data["id"] !== undefined ? _data["id"] : <any>null;
            this.name = _data["name"] !== undefined ? _data["name"] : <any>null;
        }
    }

    static fromJS(data: any, _mappings?: any): CompanyInfoModel | null {
        data = typeof data === 'object' ? data : {};
        return createInstance<CompanyInfoModel>(data, _mappings, CompanyInfoModel);
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id !== undefined ? this.id : <any>null;
        data["name"] = this.name !== undefined ? this.name : <any>null;
        return data;
    }

    clone(): CompanyInfoModel {
        const json = this.toJSON();
        let result = new CompanyInfoModel();
        result.init(json);
        return result;
    }
}

export interface ICompanyInfoModel {
    id?: string;
    name?: string;
}

function jsonParse(json: any, reviver?: any) {
    json = JSON.parse(json, reviver);

    var byid: any = {};
    var refs: any = [];
    json = (function recurse(obj: any, prop?: any, parent?: any) {
        if (typeof obj !== 'object' || !obj)
            return obj;
        
        if ("$ref" in obj) {
            let ref = obj.$ref;
            if (ref in byid)
                return byid[ref];
            refs.push([parent, prop, ref]);
            return undefined;
        } else if ("$id" in obj) {
            let id = obj.$id;
            delete obj.$id;
            if ("$values" in obj)
                obj = obj.$values;
            byid[id] = obj;
        }
        
        if (Array.isArray(obj)) {
            obj = obj.map((v, i) => recurse(v, i, obj));
        } else {
            for (var p in obj) {
                if (obj.hasOwnProperty(p) && obj[p] && typeof obj[p] === 'object')
                    obj[p] = recurse(obj[p], p, obj);
            }
        }

        return obj;
    })(json);

    for (let i = 0; i < refs.length; i++) {
        const ref = refs[i];
        ref[0][ref[1]] = byid[ref[2]];
    }

    return json;
}

function createInstance<T>(data: any, mappings: any, type: any): T | null {
  if (!mappings)
    mappings = [];
  if (!data)
    return null;

  const mappingIndexName = "__mappingIndex";
  if (data[mappingIndexName])
    return <T>mappings[data[mappingIndexName]].target;

  data[mappingIndexName] = mappings.length;

  let result: any = new type();
  mappings.push({ source: data, target: result });
  result.init(data, mappings);
  return result;
}

export class ApiException extends Error {
    message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isApiException = true;

    static isApiException(obj: any): obj is ApiException {
        return obj.isApiException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): any {
    throw new ApiException(message, status, response, headers, result);
}

}