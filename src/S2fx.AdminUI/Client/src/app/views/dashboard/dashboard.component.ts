import { Component, ElementRef, OnInit, Input, ViewChild } from '@angular/core';
import { NgForm, FormGroup } from '@angular/forms';
import { NgClass, NgStyle } from '@angular/common';
import { Router } from '@angular/router';
import { getStyle, hexToRgba } from '@coreui/coreui/dist/js/coreui-utilities';
import { CustomTooltips } from '@coreui/coreui-plugin-chartjs-custom-tooltips';
import { MetadataService, EntityService } from '../../s2fx-client/services'


@Component({
    templateUrl: 'dashboard.component.html'
})
export class DashboardComponent implements OnInit {

    items: any[]
    users: any[]

    constructor(private mdService: MetadataService, private entityService: EntityService) {
    }

    async ngOnInit() {
        let metaEntities = await this.mdService.getAllEntities();
        this.items = metaEntities

        let userResult = await this.entityService.query("Core.User", null, "new(Id, UserName, FullName, Email)")
        this.users = userResult.Entities
    }
}
