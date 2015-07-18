//#region Apache License
/**
 * Copyright 2015 StrixIT. Author R.G. Schurgers MA MSc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
//#endregion

adminModule.addRoute({ module: 'Cms', route: 'Cms/EntityAction', templateUrl: 'Cms/EntityAction' });
adminModule.addRoute({ module: 'Cms', route: 'Cms/Document/CreateMany', templateUrl: 'Cms/Document/CreateMany', controller: 'createdocumentcontroller' });
adminModule.addCrudRoute({ module: 'Cms', type: 'Html' });
adminModule.addCrudRoute({ module: 'Cms', type: 'News' });
adminModule.addCrudRoute({ module: 'Cms', type: 'Document', controller: 'documentcontroller' });
adminModule.addCrudRoute({ module: 'Cms', type: 'MailContentTemplate' });
adminModule.addCrudRoute({ module: 'Cms', type: 'MailContent', controller: 'mailcontentcontroller' });
adminModule.addCrudRoute({ module: 'Cms', type: 'Vocabulary' });