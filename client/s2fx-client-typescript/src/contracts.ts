import { IS2HttpClient } from './http'
import * as model from './model'
import { isUndefined } from 'util';

export interface IContract {

}

export abstract class AbstractContract implements IContract {

    constructor(protected readonly httpClient: IS2HttpClient) {

    }
}

////////////////////////////////////////////// Metadata Contract //////////////////////////////////////////////
export interface IMetadataContract extends IContract {
    getAllMetaEntities(): Promise<model.MetaEntity[]>
    getSingleMetaEntity(name: string): Promise<model.MetaEntity>

    getMainMenu(): Promise<model.MenuItem[]>
    getSingleView(name: string): Promise<model.ViewInfo>

    getSingleAction(id: number): Promise<model.ActionInfo>
}

export class MetadataContract extends AbstractContract implements IMetadataContract {

    async getAllMetaEntities(): Promise<model.MetaEntity[]> {
        return await this.httpClient.getAsJson('/Metadata/MetaEntity/All')
    }

    async getSingleMetaEntity(name: string): Promise<model.MetaEntity> {
        return await this.httpClient.getAsJson('/Metadata/MetaEntity/Single', {name: name})
    }

    async getMainMenu(): Promise<model.MenuItem[]> {
        let path = '/Metadata/View/MainMenu'
        return await this.httpClient.getAsJson(path)
    }

    async getSingleView(name: string): Promise<model.ViewInfo> {
        let path = '/Metadata/View/SingleView'
        return await this.httpClient.getAsJson(path, {name: name})
    }

    async getSingleAction(id: number): Promise<model.ActionInfo> {
        let path = '/Metadata/Action/Single'
        return await this.httpClient.getAsJson(path, {id: id})
    }
}

////////////////////////////////////////////// Entity Restful Contract //////////////////////////////////////////////
export interface IDynamicRestEntityContract extends IContract {
    single(entity: string, id: number): Promise<{[key: string]: any}>
    query(entity: string, queryParam?: model.EntityQueryParameter): Promise<model.EntityQueryResult>
    delete(entity: string, id: number): Promise<void>
}

export class DynamicRestEntityContract extends AbstractContract implements IDynamicRestEntityContract {

    async single(entity: string, id: number): Promise<{[key: string]: any}> {
        let path = `/Entity/${entity}/Single`
        return await this.httpClient.getAsJson(path, {id: id})
    }

    async query(entity: string, queryParam?: model.EntityQueryParameter): Promise<model.EntityQueryResult> {
        if(queryParam == undefined) {
            queryParam = {
            }
        }

        if(queryParam.offset == undefined) {
            queryParam.offset = 0
        }
        if(queryParam.limit == undefined) {
            queryParam.limit = 50
        }
        if(queryParam.select == undefined) {
            queryParam.select = "new(Id)"
        }
        let path = `/Entity/${entity}/Query`
        return await this.httpClient.getAsJson(path, queryParam)
    }

    async delete(entity: string, id: number): Promise<void> {
        let path = `/Entity/${entity}/Delete`
        return await this.httpClient.delete(path, {id: id})
    }

}
