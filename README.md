
<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

![image](https://user-images.githubusercontent.com/73213721/219876133-2880df8b-f0c1-4c8d-8d34-2ad07bd1b752.png)

Our thesis project is containing 4 main ideas which are:
1.	OPC UA Server Design (chapter 3).
2.	OPC UA Web Client Design (chapter 4).
3.	IOT 2050 implementation (chapter 5). 
4.	PLCs and Raspberry implementation (chapter 5).

The structure of the data flow thesis will be like this:
The system of the thesis will include 7 parts:
1.	MQTT Broker is deployed on Raspberry Pi 3B+.
2.	PLC S7-1200 will be the control and monitoring object.
3.	OPC UA Client is built on IoT 2050 to map PLC S7-1200 data to OPC UA Server.
4.	OPC UA Server is built using the .Net Framework. OPC UA Server acts as an intermediary for clients to communicate with each other. 
5.	The OPC UA Client is an intermediary that compiles the OPC UA variables into an API that can be read by the Frontend.
6.	The database used in the system is Azure SQL Database.
The frontend is built using .NET Blazor Server.


<p align="right">(<a href="#readme-top">back to top</a>)</p>



### Built With

This section should list any major frameworks/libraries used to bootstrap my project. 
* [![Bootstrap][Bootstrap.com]][Bootstrap-url]
* [![JQuery][JQuery.com]][JQuery-url]

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- GETTING STARTED -->
## Getting Started

OPC UA Web Platfrom requires .NET 6, available for Windows, Linux and MacOS [here](https://www.microsoft.com/net/learn/get-started).

Start cloning the project in your local machine and configure it
in order to expose your OPC UA Servers through the OPC UA Web Platform interface:

1. Clone the project in a local folder:

    `git clone https://github.com/hungn15012000/ThesisClients.git`

2. Edit the application configuration file **appsettings.json** setting all
the information relevant to the OPC UA servers you want to be exposed by the platform

    ```js
    "OPCUAServersOptions": {
        "Servers": [
          {
            "Name": "UaCppLocalServer",
            "Url": "opc.tcp://localhost:48010"
          },
          {
            "Name": "Raspberry Server",
            "Url": "opc.tcp://192.168.1.101:48010"
          }
        ]
      }
    ```

    You can edit the configuration file in order to change JWT authentication parameter too.

3. Edit the file **OPCUAWebClatform.Config.xml** in order to set all the configuration
relevant to the OPC UA Middleware embedded in the platform (which is itself 
an OPC UA Client, so it requires its own configuration).

4. Go in the project directory and run the project

    `dotnet run`

    N.B. You have to take care about running the Web Platform in *Development* or 
*Production* configuration. You can choose the configuration setting the 
environment variable **ASPNETCORE_ENVIRONMENT**, as explained [here](https://docs.microsoft.com/it-it/aspnet/core/fundamentals/environments).

### Run on Docker container

It is possible to run OPC UA Web Platform on Docker container with the following command

`docker run --rm -it -p 5000:80 marsala/opcua-web-platform:1.0.0`

### Troubleshooting

You may occur in error like "DataSet Not Available" even if all the ip addresses or your OPC UA Server are perfectly configured. Be aware you have configured the platform **Instance Certificate** in the OPC UA Servers **Trusted** certificate store.

## Examples

In the following will be highlighted some common use cases for the OPC UA Web Platform. Remember that all
request to the Api with the initial enpoint **/api/** require an authentication token. A valid token can be
obtained with a simple request to the following API endpoint:

`POST http://{{base_url}}/auth/authenticate`

The response will contain a valid token that must be included in an header **"Authorization"** like shown
in the following examples.

This is an example request of authentication using the default Authentication service mock:

```
curl -X POST \
  http://localhost:5000/auth/authenticate \
  -F username=admin \
  -F password=password
```

### Read the standard OPC UA Server *Object* Node

Suppose that the DataSet with dataset-id = 3 correspond to the OPC UA Server exposed by the platform
you are interested. So, if you want to start browsing the data set you have to make a request like 
the following:

`GET http://{{base_url}}/api/data-sets/3/nodes`

You can make a request with:

```
curl -X GET http://{{YOUR-URL}}:5000/api/data-sets/2/nodes 
  -H 'Authorization: Bearer {YOUR-AUTHENTICATION-TOKEN}'
```

It will return the response:

```js
{
    "node-id": "0-85",
    "name": "Objects",
    "type": "folder",
    "edges": [
        {
            "node-id": "0-2253",
            "name": "Server",
            "Type": "object",
            "relationship": "Organizes"
        },
        {
            "node-id": "3-BuildingAutomation",
            "name": "BuildingAutomation",
            "Type": "folder",
            "relationship": "Organizes"
        },
        {
            "node-id": "2-Demo",
            "name": "Demo",
            "Type": "folder",
            "relationship": "Organizes"
        },
        {
            "node-id": "5-Demo",
            "name": "DemoUANodeSetXML",
            "Type": "folder",
            "relationship": "Organizes"
        }
    ]
}
```

### Read the a Node

Read the state of a Variable Node:

`GET http://{{base_url}}/api/data-sets/3/nodes/2-Demo.Static.Scalar.WorkOrder`

You can make a request with:

```
curl -X GET \
  http://{YOUR-URL}/api/data-sets/3/nodes/2-Demo.Static.Scalar.WorkOrder \
  -H 'Authorization: Bearer {YOUR-AUTHENTICATION-TOKEN}' \
```

It will return the response:

```js
{
    "node-id": "2-Demo.Static.Scalar.WorkOrder",
    "name": "WorkOrder",
    "type": "variable",
    "value": {
        "ID": "9240890a-6ea8-41fc-8e84-f47edd3e3595",
        "AssetID": "123-X-Y-Z",
        "StartTime": "2018-04-20T14:24:39.085941Z",
        "NoOfStatusComments": 3,
        "StatusComments": [
            {
                "Actor": "Wendy Terry",
                "Timestamp": "2018-04-20T14:24:39.085941Z",
                "Comment": {
                    "Locale": "en-US",
                    "Text": "Mission accomplished!"
                }
            },
            {
                "Actor": "Gavin Mackenzie",
                "Timestamp": "2018-04-20T14:24:39.085941Z",
                "Comment": {
                    "Locale": "en-US",
                    "Text": "I think clients would love this."
                }
            },
            {
                "Actor": "Phil Taylor",
                "Timestamp": "2018-04-20T14:24:39.085941Z",
                "Comment": {
                    "Locale": "en-US",
                    "Text": "And justice for all."
                }
            }
        ]
    },
    "value-schema": {
        "type": "object",
        "properties": {
            "ID": {
                "type": "string"
            },
            "AssetID": {
                "type": "string"
            },
            "StartTime": {
                "type": "string"
            },
            "NoOfStatusComments": {
                "type": "number"
            },
            "StatusComments": {
                "type": "array",
                "items": {
                    "type": "object",
                    "properties": {
                        "Actor": {
                            "type": "string"
                        },
                        "Timestamp": {
                            "type": "string"
                        },
                        "Comment": {
                            "type": "object",
                            "properties": {
                                "Locale": {
                                    "type": "string"
                                },
                                "Text": {
                                    "type": "string"
                                }
                            }
                        }
                    }
                },
                "minItems": 3,
                "maxItems": 3
            }
        }
    },
    "status": "Good",
    "deadBand": "None",
    "minimumSamplingInterval": 0,
    "edges": []
}
```

### Write a new value for a Variable Node

Update the value of a Variable Node:

`POST http://{{base_url}}/api/data-sets/3/nodes/2-Demo.Static.Scalar.WorkOrder { "value": <YOUR-NEW-VALUE> }`

You can make a request with:

```
curl -X POST \
  http://{YOUR-URL}/api/data-sets/3/nodes/2-Demo.Static.Scalar.WorkOrder \
  -H 'Authorization: Bearer {YOUR-AUTHENTICATION-TOKEN} \
  -d '{
  "value": {
        "ID": "9240890a-6ea8-41fc-8e84-f47edd3e3595",
        "AssetID": "123-X-Y-Z",
        "StartTime": "2018-04-20T14:24:39.085941Z",
        "NoOfStatusComments": 2,
        "StatusComments": [
            {
                "Actor": "Dylan Thomas",
                "Timestamp": "2018-04-20T14:24:39.085941Z",
                "Comment": {
                    "Locale": "en-US",
                    "Text": "Do not go gentle into that good night"
                }
            },
            {
                "Actor": "William Shakespeare",
                "Timestamp": "2018-04-20T14:24:39.085941Z",
                "Comment": {
                    "Locale": "en-US",
                    "Text": "There is nothing either good or bad, but thinking makes it so."
                }
            }
        ]
    }
}'
```

It worth noting how a client not compliant with the OPC UA specification is able to write new well-formed values
with the aid of JSON Schemas.

### Prerequisites

1. AutoMapper
2. BlazorLocalStorage
3. 

### Installation

_Below is an example of how you can instruct your audience on installing and setting up your app. This template doesn't rely on any external dependencies or services._

1. Get a free API Key at [https://example.com](https://example.com)
2. Clone the repo
   ```sh
   git clone https://github.com/your_username_/Project-Name.git
   ```
3. Install NPM packages
   ```sh
   npm install
   ```
4. Enter your API in `config.js`
   ```js
   const API_KEY = 'ENTER YOUR API';
   ```

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- USAGE EXAMPLES -->
## Usage

Use this space to show useful examples of how a project can be used. Additional screenshots, code examples and demos work well in this space. You may also link to more resources.

_For more examples, please refer to the [Documentation](https://example.com)_

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- ROADMAP -->
## Roadmap

- [x] Add Changelog
- [x] Add back to top links
- [ ] Add Additional Templates w/ Examples
- [ ] Add "components" document to easily copy & paste sections of the readme
- [ ] Multi-language Support
    - [ ] Chinese
    - [ ] Spanish

See the [open issues](https://github.com/othneildrew/Best-README-Template/issues) for a full list of proposed features (and known issues).

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTRIBUTING -->
## Contributing

- [Integration of OPC UA into a web-based platform to enhance interoperability (ISIE 2017)](https://ieeexplore.ieee.org/document/8001417/)
- [OPC UA integration into the web (IECON 2017)](https://ieeexplore.ieee.org/document/8216590/)
- [A web-based platform for OPC UA integration in IIoT environment (ETFA 2017)](https://ieeexplore.ieee.org/document/8247713/)
- [Integrating OPC UA with web technologies to enhance interoperability (Computer Standards & Interfaces 2018 - Elsevier)](https://doi.org/10.1016/j.csi.2018.04.004)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

top">back to top</a>)</p>



<!-- CONTACT -->
## Contact

Nguyen Xuan Hung - hung.nguyenxuan1501@gmail.com

Project Link: [[https://github.com/hungn15012000/](https://github.com/hungn15012000/))
](https://github.com/hungn15012000/)
<p align="right">(<a href="#readme-top">back to top</a>)</p>




