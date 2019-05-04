import { S2AxiosHttpClient } from "../src/http.axios"
import { DynamicRestEntityContract, MetadataContract } from "../src/contracts"

let httpClient = new S2AxiosHttpClient('http://localhost:59129/Default')

jest.setTimeout(30000)

describe("MetadataContrct test", () => {
     let metadataRpcService = new MetadataContract(httpClient)

    it("MetadataContract Can get all meta entities", async () => {
        let metaEntities = await metadataRpcService.getAllMetaEntities()
        expect(metaEntities.length).toBeGreaterThan(0)
    })

    it("MetadataContract can get single meta entitity", async () => {
        let me = await metadataRpcService.getSingleMetaEntity('Core.User')
        expect(me.Name).toEqual('Core.User')
    })

    it("MetadataContract can get main menu", async () => {
        let menus = await metadataRpcService.getMainMenu()
        expect(menus.length).toBeGreaterThan(0)
        expect(menus[0].Id).toBeGreaterThan(0)
    })

    it('MetadataContract can get user list view', async () => {
        const VIEW_NAME = 'View.Core.User.List'
        let viewInfo = await metadataRpcService.getSingleView(VIEW_NAME)
        expect(viewInfo.View.ViewType).toEqual('ListView')
        expect(viewInfo.Name).toEqual(VIEW_NAME)
        expect(viewInfo.MetaFields.length).toBeGreaterThan(0)
    })

    it("MetadataContract can get first action", async () => {
        let action = await metadataRpcService.getSingleAction(1)
        expect(action.Id).toEqual(1)
    })

})


describe("DynamicEntityContract test", () => {

    /*
    it("DynamicEnttiyContract can query", async () => {
        let rpc = new DynamicRestEntityContract(httpClient)
        let queryParam = {
            filter: 'it.UserName == "admin"',
            select: 'new(Id,UserName,Email)'
        }
        let queryResult = await rpc.query('Core.User', queryParam)
        expect(queryResult.Total).toEqual(1)
        expect(queryResult.Count).toEqual(1)
        expect(queryResult.Offset).toEqual(0)
        expect(queryResult.Entity).toEqual('Core.User')
        expect(queryResult.Entities).toBeDefined()
        expect(queryResult.Entities.length).toEqual(1)
    })
    */

    /*
    it("DynamicEntityContract can get single", async() => {
        let contract = new DynamicRestEntityContract(httpClient)
        let record = await contract.single('Core.User', 1)
        expect(record.Id).toEqual(1)
        expect(record.UserName).toEqual('admin')
    })
    */
})
