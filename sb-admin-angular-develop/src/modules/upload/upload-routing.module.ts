/* tslint:disable: ordered-imports*/
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import * as uploadcontainers from './containers';

/* Module */
import { UploadModule } from './upload.module';

import { SBRouteData } from '@modules/navigation/models';

/* Routes */
export const ROUTES: Routes = [
    {
        path: '',
        canActivate: [],
        component: uploadcontainers.UploadComponent,
        data: {
            title: 'Upload File',
            breadcrumbs: [
                {
                    text: 'Dashboard',
                    link: '/dashboard',
                },
                {
                    text: 'Upload',
                    active: true,
                },
            ],
        } as SBRouteData,
    },
];

@NgModule({
    imports: [UploadModule, RouterModule.forChild(ROUTES)],
    exports: [RouterModule],
})
export class UploadRoutingModule {}
