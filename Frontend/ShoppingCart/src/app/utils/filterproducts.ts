import { IProductModel } from '../products/IProductModel';

export abstract class Filterproducts {
  static applyFilter(
    productCollection: IProductModel[],
    searchValue: string
  ): IProductModel[] {
    let filteredproductCollection: IProductModel[] = [];
    if (
      productCollection != null &&
      productCollection.length > 0 &&
      searchValue != ''
    ) {
      for (let product of productCollection) {
        if (
          product.productName.toLowerCase().includes(searchValue.toLowerCase())
        ) {
          filteredproductCollection.push(product);
        }
      }
    }
    return filteredproductCollection;
  }
}
