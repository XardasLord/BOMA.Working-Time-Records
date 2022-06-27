import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { LayoutModule } from '@angular/cdk/layout';
import { AppRoutingModule } from './modules/app-routing.module';
import { AppNgxsModule } from './modules/app-ngxs.module';
import { AppMaterialModule } from './modules/app-material.module';
import { NavigationComponent } from './ui/components/navigation/navigation.component';

@NgModule({
  declarations: [NavigationComponent],
  imports: [
    CommonModule,
    LayoutModule,
    AppMaterialModule,
    AppRoutingModule,
    AppNgxsModule,
    ReactiveFormsModule,
  ],
  exports: [
    CommonModule,
    AppRoutingModule,
    AppMaterialModule,
    NavigationComponent,
    ReactiveFormsModule,
  ],
})
export class CoreModule {}
