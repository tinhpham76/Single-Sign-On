import { Injectable } from "@angular/core";
import { BaseService } from './base.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Role } from '../models/role.model';
import { environment } from '@environments/environment';
import { catchError, map } from 'rxjs/operators';
import { Pagination } from '../models/pagination.model';
import { IdentityResource } from '../models/identity-resource.model';

@Injectable({ providedIn: 'root' })
export class IdentityResourceServices extends BaseService {
    private _sharedHeaders = new HttpHeaders();
    constructor(private http: HttpClient) {
        super();
        this._sharedHeaders = this._sharedHeaders.set('Content-Type', 'application/json');
    }
    add(entity: IdentityResource) {
        return this.http.post(`${environment.apiUrl}/api/IdentityResources`, JSON.stringify(entity), { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    update(name: string, entity: IdentityResource) {
        return this.http.put(`${environment.apiUrl}/api/IdentityResources/${name}`,
            JSON.stringify(entity), { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    getDetail(name: string) {
        return this.http.get<IdentityResource>(`${environment.apiUrl}/api/IdentityResources/${name}`,
            { headers: this._sharedHeaders }).pipe(catchError(this.handleError));

    }
    getAllPaging(filter, pageIndex, pageSize) {
        return this.http.get<Pagination<IdentityResource>>(`${environment.apiUrl}/api/IdentityResources/filter?filter=${filter}&pageIndex=${pageIndex}&pageSize=${pageSize}`, { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    delete(name: string) {
        return this.http.delete(`${environment.apiUrl}/api/IdentityResources/${name}`, { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    getIdentityResourceClaims(name: string) {
        return this.http.get(`${environment.apiUrl}/api/IdentityResources/${name}/identityClaims`, { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    addIdentityResourceClaim(name: string, entity: IdentityResource) {
        return this.http.post(`${environment.apiUrl}/api/IdentityResources/${name}/identityClaims`,
            JSON.stringify(entity), { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    deleteIdentityResourceClaim(name: string, type: string) {
        return this.http.delete(`${environment.apiUrl}/api/IdentityResources/${name}/identityClaims/${type}`,
            { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
}