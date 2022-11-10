import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import * as uploadcontainers from './containers';
import * as uploadcomponents from './components';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppCommonModule } from '../app-common/app-common.module';
import { NavigationModule } from '../navigation/navigation.module';



@NgModule({
    declarations: [...uploadcontainers.containers, ...uploadcomponents.components],
    exports: [...uploadcontainers.containers, ...uploadcomponents.components],
  imports: [
      CommonModule,
      RouterModule,
      ReactiveFormsModule,
      FormsModule,
      AppCommonModule,
      NavigationModule,
    ]
})
export class UploadModule { }
