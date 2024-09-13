import { Component, inject } from '@angular/core';
import { CategoryDetailComponent } from '../category-detail/category-detail.component';
import { CategoriesClient } from 'src/app/modules/generated-clients/api-service';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrl: './category-list.component.scss',
  providers: [CategoriesClient]
})
export class CategoryListComponent {
  detailComponent = CategoryDetailComponent;
  pageId = '093807c1-a55d-4e3d-4399-08dcd292d151'

  entityClient: CategoriesClient = inject(CategoriesClient);
}
