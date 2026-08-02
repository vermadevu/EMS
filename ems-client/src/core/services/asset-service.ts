import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { environment } from '../../environments/environment';
import { AssetListState } from '../models/asset-list-state';
import { Observable } from 'rxjs';
import { PagedResult } from '../../features/employees/models/paged-result';
import { AssetListItem } from '../models/asset-list-item';
import { Asset } from '../models/asset';
import { CreateAsset } from '../models/create-asset';
import { UpdateAsset } from '../models/update-asset';

@Service()
export class AssetService {
    private readonly http = inject(HttpClient);

    private readonly baseUrl = `${environment.apiUrl}/asset`;

    getAssets(
        state: AssetListState
    ): Observable<PagedResult<AssetListItem>> {

        let params = new HttpParams()
            .set('pageNumber', state.pageNumber)
            .set('pageSize', state.pageSize)
            .set('sortBy', state.sortBy)
            .set('sortDirection', state.sortDirection);

        if (state.search) {
            params = params.set('search', state.search);
        }

        if (state.assetType != null) {
            params = params.set('assetType', state.assetType);
        }

        if (state.status != null) {
            params = params.set('status', state.status);
        }

        return this.http.get<PagedResult<AssetListItem>>(
            this.baseUrl,
            { params }
        );
    }

    getAsset(id: number) {
        return this.http.get<Asset>(
            `${this.baseUrl}/${id}`
        );
    }

    create(dto: CreateAsset) {
        return this.http.post<Asset>(
            this.baseUrl,
            dto
        );
    }

    update(id: number, dto: UpdateAsset) {
        return this.http.put<Asset>(
            `${this.baseUrl}/${id}`,
            dto
        );
    }

    delete(id: number) {
        return this.http.delete(
            `${this.baseUrl}/${id}`
        );
    }

    assign(id: number, employeeId: number) {
        return this.http.patch(
            `${this.baseUrl}/${id}/assign`,
            { employeeId }
        );
    }

    return(id: number) {
        return this.http.patch(
            `${this.baseUrl}/${id}/return`,
            {}
        );
    }

    getByEmployee(employeeId: number) {
        return this.http.get<Asset[]>(
            `${this.baseUrl}/employee/${employeeId}`
        );
    }

    getMyAssets() {
        return this.http.get<Asset[]>(
            `${this.baseUrl}/me`
        );
    }
}
