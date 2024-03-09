README

Link para o board do Projeto:

https://github.com/users/Mizuiky/projects/11/views/1

Tags:
- v1.0  https://github.com/Mizuiky/TetrisInventory/releases/tag/v1.0
  
## Supreme Tile Inventory

(Disclaimer: Essa 'documentação' só está em portugues porque é mais simples de explicar. Normalmente, faria em ingles.)

### Ideia básica:

Um inventário que possa conter itens com diversos tamanhos diferentes, e poder organizar entre eles.
No fim, o proprio inventário se torna um mini quebra-cabeça para o jogador, deixando o jogo mais estratégico do que apenas
uma bolsa da hermione.

### Observacoes:
- Dentro da pasta Resources em Assets/Resources/SaveData deixei o json limpo do inventario e da configuracao dos items adicionados
- estava usando o persistent data path do meu pc mas como no de voces sera diferente entao seria so atualizar os jsons criados em parsistentData path + SaveData/ utilizando os jsons do Resources folder

persistent data path aqui no meu : C:\Users\gabri\AppData\LocalLow\DefaultCompany\Inventory\SaveData

---

## Descoberta:
Isso obviamente é mais dificil do que parece, justamente porque os itens precisam manter uma forma de 'encaixar' no inventário,
de tal forma que ele saiba qual peça está em cada lugar, para possibilitar com que as outras também possam ser encaixadas.

A primeira abordagem, foi tentar utilizar collider para as peças, para umas não poderem tocar umas nas outras, mas isso não funcionou
tão bem porque por exemplo usando imagem estilo L o collider iria cobrir a imagem por inteiro que 'e um retangulo e nao somente o formato em L em si, entao descartei esta possibilidade

Uma segunda abordagem, foi em usar matrizes para o inventário e para a configuracao do item.
Dessa forma, cada peça teria sua propria "configuração" que assume no inventário (eliminando o problema com o transparente).
E o inventário, por sua vez, consegue sempre determinar qual peça consegue encaixar em cada lugar.

Computacionalmente isso não é performático (fazer vários loops em matriz), mas como o tamanho geral do inventário não é grande apesar de ser configuravel
não creio que será um problema.
Unico ponto negativo (até o momento) é que o código acaba ficando bem mais complexo.

## Adicionando primeira peca ao inventario

A ideia foi para cada novo item adicionado, criar uma configuracao formada por 0 e 1 onde 1 representa na matriz de configuracao o local onde existe um quadrado 64x64 que compoe a peca
entao seguindo o exemplo do L ficaria:

|   A   |   B   |   C   |
|-------|-------|-------|      
| 0,0   | 0,1   | 0,2   |       
| 1,0   | 1,1   | 1,2   |       


# = L    

|   A   |   B   |   C   |
|-------|-------|-------|      
|   1   |   0   |   0   |       
|   1   |   1   |   1   |  

### 1 - Verificação de Disponibilidade do Slot:
- Percorremos cada slot no inventário para verificar se está disponível, ou seja, se não possui itens adicionados.

### 2 - Verificação da Configuração do Item:
- Se o slot estiver disponível, verificamos se a configuração do item pode se encaixar a partir desse slot.
- Calculamos a quantidade de colunas e linhas disponíveis a partir do slot verificado.

### 3 - Determinação de Colunas e Linhas Disponíveis:
- Calculamos as colunas disponíveis a partir do slot atual subtraindo a coluna atual da quantidade total de colunas no inventário.
- Calculamos as linhas disponíveis a partir do slot atual subtraindo a linha atual da quantidade total de linhas no inventário.

### 4 - Restrições de Configuração do Item:
- Se o número de colunas da matriz de configuração do item for maior que as colunas disponíveis, não é possível adicionar o item.
- Se a quantidade de linhas da matriz de configuração do item for maior que as linhas disponíveis, não é possível adicionar o item.

### 5 - Lógica de Encaixe do Item:
a. Se o valor na configuração do item for zero (indicando posição vazia para o item, como no exemplo do "L"):
- Se estiver na última coluna da configuração do item:
  - Ajustamos a coluna do auxiliar do inventário para a coluna inicial.
  - Continuamos a verificação nas próximas colunas quando a linha muda.

#### Caso contrário:
- Incrementamos a coluna atual e passamos para a próxima iteração na configuração do item.

b. Se encontrar o numero um na configuração e a posição do inventário na linha e coluna atual tiver um item diferente do item atual na configuração:
- Significa que o slot do inventário está ocupado.
- Passamos para a próxima posição do inventário para verificar se a configuração encaixa.

c. Se o slot estiver disponível:
- Adicionamos esta nova posição à lista de posições.
- Incrementamos a coluna indo para a próxima.
- Se estivermos na última coluna da matriz de configuração do item:
  - Na próxima iteração, mudaremos de linha, ajustamos a coluna atual para a coluna inicial.
  - Incrementamos o auxiliar da linha para o inventário.
  - Slot atual é armazenado para uso posterior.

### 6 - Instanciação e Adição do Item:
- Instanciamos o item, atualizando dados e propriedades e subscrevendo a eventos para verificar a próxima posição disponível.
- A posição do item é definida com base no slot atual e nas dimensões do slot do inventário.
- Adicionamos o item a cada posição da lista de posições encontradas anteriormente, caso a configuração atual da posição não seja vazia.
- Incrementamos a quantidade do item para 1.

### 7 - Atualização e Salvamento:
-  o item à lista de itens no inventário.
- Atualizamos os dados do item e do inventário, sendo passados para o Item Manager, que chamará o Save Manager para atualizar esses dados no JSON posteriormente.

---

## Tool para Adicionar novos items

Campos:

- Nome do item
- ID
- Type(None, Material, Weapon, Ammo, Consumable)
- Sprite(sprite referente ao item)
- Descricao do item
- Imagem do inventario(selecionar uma das pecas dentro da pasta Assets/UI/InventoryItems
- ImageConfig(configuracao em formato de matriz da peca do inventario)
   - Linhas
   - Colunas

Ao preencher todos os campos aparecerá um botao para criar o item

- O item builder cria prefabs com as configuracoes setadas para um item normal e um item de inventario
armazena esses prefabs na Pasta Resources/Prefabs/InventoryItems  e  Resources/Prefabs/Items

- Os dados dos items criados sao salvos em json dentro do caminho C:\Users\[Seu usuario]\AppData\LocalLow\DefaultCompany\Inventory\GameData
- O arquivo referente aos items se chama ItemData, e para os de inventario, InventoryData

- Quando o jogo é iniciado, é feito o load dos dados desses arquivos, alem disso é feito o load de cada um dos prefabs de items e inventory items
o dados correspondendte de cada um é encaixado e o item manager adiciona esses itens e dados em listas para que sejam de facil acesso.

<p align="center">

### Botao na barra de ferramentas para abrir a tool

<img src="Inventory\Assets\ReadMe\Images\item creator path.jpg" alt="Tool">

### Tool para criar os itens

<img src="Inventory/Assets/ReadMe/Images/tool.png" alt="Tool">

### Exemplo da configuracao do item criado dentro do JSON

<img src="Inventory/Assets/ReadMe/Images/JsonConfig.png" alt="Tool">

</p>

---

## Tool para criar o inventario

Aqui temos um Inventory Builder, que a partir de campos cria um novo inventario

Campos:
- Prefab do slot que compoem o inventario
- Parent para encaixar o inventario na UI
- O parent para poder ser um container para os itens do inventario
- Sprite com o slot normal
- Sprite com slot iluminado
- Posicao x inicial 
- Posicao y inicial
- Quantidade de linhas
- Quantidade de colunas

Para cada indice da matriz linha e coluna é instanciado um novo slot na posicao x,y, alem disso o slot é inicializado com seu indice posicao e dados salvos
Apos o termino da matriz o inventario é inicializado com seus slots, parent, e setado no UIController

---

## Rotacionar items

- Espace key rotaciona em 90 graus a peca para a esquerda

---

## Como spawnar o primeiro item

- Na hierarchy preencher os campos do inventory builder para criar um novo inventario, depois iniciar o jogo
- No prefab Spawner, digitar um numero de 0 a 17 que sao a quantidade de items cadastrados  no campo Item Id To Spawn
- Na game Scene clicar no botao rosa Spawn item.
- Deixei o inventario comecando na posicao 0,0 e com 5 linhas e 5 colunas.


## Adicao e movimentacao das pecas no inventario

Para adicionar precisamos usar a constante:

### Vector3(wImg/2 - wSlot/2, -hImg/2 + hSlot/2) que encaixa a peca.
### a nova posicao e a posicao atual do slot + a constante tanto no eixo x como no y ficando:

var newPosition = new Vector3(slot.Position.x + _constant.x, slot.Position.y + _constant.y, _rect.localPosition.z);

onde:

- wSlot = Width do Slot.
- hSlot = Height do Slot.

e para movimentar usamos:

- Direita: posicao local + wSlot NO EIXO x
- Esquerda: posicao local - wSlot NO EIXO x

- Emcima: posicao local + hSlot NO EIXO y
- Embaixo: posicao local - hSlot NO EIXO y

Para verificar os casos de borda temos que achar a quantidade de colunas disponiveis(nao possuem item)a partir do slot atual.
isso se da subtraindo a quantidade total de colunas do inventario menos a coluna atual do meu slot que estou verificando se posso encaixar o item nele.

com isso:

- Se a quantidade de colunas do meu item for maior que o numero de colunas disponiveis do meu inventario , eu nao posso adicionar o item, caso contario ele tem a chance de ser adicionado.
- Se a quantidade de linhas do meu item for maior que o numero de linhas disponiveis do meu inventario, eu tambem nao posso adicionar o item.
---

## Itens para fazer  

## Continuacao da tool de adicionar itens mas agora para edita-los

Adicionar continuacao da tool de criacao de itens mas agora para poder edita-los
é possivel editar via json mas nao é pratico para game designers, entao uma nova tool
que pudesse pegar a lista de items disponiveis e setar um um campo, e eu poder escolher o item pelo id
e todos os itens aparecessem para edtar e no final um botao de update que alem de atualizar no item manager atualizasse tambem no
json atraves do save manager seria o ideal;

poderia ter usado scritable objects mas acredito que uma tool fica mais intuitivo para um game designer.

---

## Melhorar de forma visual a matriz de mapeamento da peca  

- A matriz de 1 e 0 mapeia os campos da peca que nao sao transparentes
- Seria legal ser possivel adicionar a linha e coluna e elas formassem o formato da matriz visualmente na tool para o game designer,
assim ele so encaixaria cada bloco formando a peca, no exemplo do L ele adicionaria blocos 64x64 da imagem somente onde existe 
blocos na peca em L, seria uma forma mais visual de mapeamento

---

## Salvar os dados de cada slot do inventario para poder fazer o load deles no inicio do jogo

---

## O ideal seria salvar os dados do player em um servidor e dar load deles a partir disso, com os dados carregados salvar eles no persistent data path que sera o caminho para o computador do usuario

---

## Completar a documentacao, deixa-la mais bonita e salvar em PDF

---


