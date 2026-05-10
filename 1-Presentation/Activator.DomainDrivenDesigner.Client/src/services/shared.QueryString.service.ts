import { Injectable } from "@angular/core";
import { ActivatedRoute } from "@angular/router";


@Injectable({
  providedIn: "root"
})
export class QueryStringService {

  constructor(
    private route: ActivatedRoute) {

  }

  public Get(key: string): string | null {

    let valueInternal = this.GetValueSpaInternal(key);
    let valueExternal = this.GetValueSpaExternal(key);

    return valueInternal != null
      ? valueInternal
      : valueExternal != null
        ? valueExternal
        : null;
  }

  private GetValueSpaInternal(key: string): string | null {
    return this.route.snapshot.queryParamMap.get(key);
  }

  private GetValueSpaExternal(key: string): string | null { // with hash in url

    let queryStringGroups = new Map<string, string>();

    let fragment = this.route.snapshot.fragment;
    let queryStringRaw = fragment?.split("?")[1]; // get all query strings.
    let queryStrings = queryStringRaw?.split("&");

    queryStrings?.forEach(qs => {
      let queryStringGroup = qs.split("=");
      queryStringGroups.set(queryStringGroup[0], queryStringGroup[1]);
    })

    let value = queryStringGroups.get(key);

    return value != undefined ? value : null;
  }
}
